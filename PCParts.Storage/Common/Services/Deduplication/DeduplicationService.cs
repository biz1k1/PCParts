using Microsoft.Extensions.Caching.Memory;

namespace PCParts.Storage.Common.Services.Deduplication;

public class DeduplicationService : IDeduplicationService
{
    private readonly IMemoryCache _memoryCache;

    public DeduplicationService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public bool IsDuplicate(string messageId)
    {
        if (_memoryCache.TryGetValue(messageId, out _))
            return true;

        _memoryCache.Set(messageId, true, TimeSpan.FromMinutes(5));
        return false;
    }

    public string GetMessageId(string cacheKey)
    {
        if (_memoryCache.TryGetValue(cacheKey, out var cachedValue))
        {
            return cachedValue.ToString();
        }

        return string.Empty;
    }
}