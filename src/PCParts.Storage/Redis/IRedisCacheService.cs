namespace PCParts.Storage.Redis
{
    public interface IRedisCacheService
    {
        Task SetAsync<T>(string key, T value);
        Task<T?> GetAsync<T>(string key);
        Task<byte[]?> GetBytesAsync(string key);
        Task RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);

    }
}
