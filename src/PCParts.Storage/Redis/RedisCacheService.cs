using System.Text.Json;
using Microsoft.Extensions.Options;
using PCParts.Storage.Common.Polly;
using StackExchange.Redis;

namespace PCParts.Storage.Redis;

public class RedisCacheService : IRedisCacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IPolicyFactory _policyFactory;
    private readonly RedisCacheOptions _options;
    public RedisCacheService
        (
            IConnectionMultiplexer connectionMultiplexer,
            IOptions<RedisCacheOptions> options,
            IEnumerable<IPolicyFactory> policies)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _policyFactory = policies.FirstOrDefault(x => x is RedisPolicyFactory);

        _options = options.Value;
    }

    /// <param name="value">Can be a C# obj or byte[]</param>
    public async Task SetAsync<T>(string key, T value)
    {
        var db = _connectionMultiplexer.GetDatabase();
        byte[] bytes;

        bytes = value is byte[] b ? b : JsonSerializer.SerializeToUtf8Bytes(value);

        var policy = _policyFactory.GetPolicy<bool>();
        await policy.ExecuteAsync(async () =>
        {
            await db.StringSetAsync(key, bytes, _options.DefaultTtl);
            return true;
        });
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var policy = _policyFactory.GetPolicy<T?>();
        return await policy.ExecuteAsync(async () =>
        {
            var value = await db.StringGetAsync(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        });
    }

    public async Task<byte[]?> GetBytesAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var policy = _policyFactory.GetPolicy<byte[]?>();
        return await policy.ExecuteAsync(async () =>
        {
            var value = await db.StringGetAsync(key);
            return value.HasValue ? (byte[])value : null;
        });
    }

    public async Task RemoveAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var value = await db.KeyExistsAsync(key);
        return value;
    }
}
