using Microsoft.Extensions.Caching.Memory;
using PCParts.API.Extension.SemaphorePoolProvider;
using PCParts.Shared.Monitoring.Metrics;
using PCParts.Storage.Redis;

namespace PCParts.API.Extension.Middlewares;

public class CachingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRedisCacheService _redisCacheService;
    private readonly IMemoryCache _memoryCache;
    private readonly SemaphorePool _semaphorePool = new(128);
    private readonly IAppMetrics _metrics;

    private static readonly Action<ILogger, string, string, Exception> _logException =
        LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(1, "UnhandledException"),
            "Error has happened with {RequestPath}, the message is {ErrorMessage}");


    public CachingMiddleware(
        RequestDelegate next,
        IRedisCacheService redisCacheService,
        IMemoryCache memoryCache,
        IAppMetrics metrics)
    {
        _next = next;
        _redisCacheService = redisCacheService;
        _memoryCache = memoryCache;
        _metrics = metrics;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ILogger<CachingMiddleware> logger)
    {
        if (!HttpMethods.IsGet(context.Request.Method)||
            context.Request.Path.StartsWithSegments("/internal/metrics"))
        {
            await _next(context);
            return;
        }
        var cacheKey = $"{context.Request.Method}:{context.Request.Path}:{context.Request.QueryString}";

        SemaphoreSlim semaphore = _semaphorePool.GetOrAddSemaphore(cacheKey);
        await semaphore.WaitAsync();

        Stream? originalBodyStream = null;
        MemoryStream? memoryStream = null;

        try
        {
            if (_memoryCache.TryGetValue(cacheKey, out bool existsFlag) && existsFlag)
            {
                var cachedBytes = await _redisCacheService.GetBytesAsync(cacheKey);
                if (cachedBytes is not null)
                {
                    context.Response.ContentType = "application/json; charset=utf-8";
                    context.Response.Headers["Cache-Control"] = "cache";
                    await context.Response.Body.WriteAsync(cachedBytes);

                    _metrics.IncrementCount("cache.hit",1);

                    return;
                }
            }

            originalBodyStream = context.Response.Body;
            memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            if (context.Response.StatusCode is StatusCodes.Status200OK)
            {
                var bytes = memoryStream.ToArray();
                await _redisCacheService.SetAsync(cacheKey, bytes);

                _memoryCache.Set(cacheKey, true, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(30)
                });

                context.Response.Headers["Cache-Control"] = "no-cache";

                _metrics.IncrementCount("cache.miss", 1);
            }
        }
        catch (Exception exception)
        {
            _logException(logger, context.Request.Path.Value!, exception.Message, exception);
        }
        finally
        {

            semaphore.Release();

            if (memoryStream is not null && originalBodyStream is not null)
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;

                await memoryStream.DisposeAsync();
            }
        }
    }
}
