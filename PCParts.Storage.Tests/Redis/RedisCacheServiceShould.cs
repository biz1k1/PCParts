using PCParts.Domain.Entities;
using PCParts.Storage.Redis;
using System.Text.Json;
using FluentAssertions;

namespace PCParts.Storage.Tests.Redis;

public class RedisCacheServiceShould : IClassFixture<RedisContainerFixture>
{
    private const string CACHEKEY = "GET/Categories";
    private readonly RedisCacheService _cacheService;
    public RedisCacheServiceShould(
        RedisContainerFixture fixture)
    {
        _cacheService = fixture.Service!;
    }
    [Fact]
    public async Task SetAsyncEntity()
    {
        var category = new Category()
        {
            Id = new Guid()
        };
        await _cacheService.SetAsync(CACHEKEY, category);

        var returnCategory = await _cacheService.GetAsync<Category>(CACHEKEY);

        returnCategory.Should().NotBeNull();
        returnCategory!.Id.Should().Be(category.Id);
    }

    [Fact]
    public async Task SetAsyncBytes()
    {
        var category = new Category()
        {
            Id = new Guid()
        };

        var bytes = JsonSerializer.SerializeToUtf8Bytes(category);

        await _cacheService.SetAsync(CACHEKEY, bytes);

        var returnCategory = await _cacheService.GetAsync<Category>(CACHEKEY);

        returnCategory.Should().NotBeNull();
        returnCategory!.Id.Should().Be(category.Id);
    }

    [Fact]
    public async Task GetAsync()
    {
        var category = new Category()
        {
            Id = new Guid()
        };

        var bytes = JsonSerializer.SerializeToUtf8Bytes(category);

        await _cacheService.SetAsync(CACHEKEY, bytes);

        var returnCategory = await _cacheService.GetAsync<Category>(CACHEKEY);

        returnCategory.Should().NotBeNull();
        returnCategory!.Id.Should().Be(category.Id);
    }

    [Fact]
    public async Task GetBytesAsync()
    {
        var category = new Category()
        {
            Id = new Guid()
        };

        var bytes = JsonSerializer.SerializeToUtf8Bytes(category);

        await _cacheService.SetAsync(CACHEKEY, bytes);

        var returnCategoryBytes = await _cacheService.GetBytesAsync(CACHEKEY);

        returnCategoryBytes.Should().NotBeNull();
        returnCategoryBytes.Should().BeEquivalentTo(bytes);

        var returnCategory = JsonSerializer.Deserialize<Category>(returnCategoryBytes!);
        returnCategory.Should().BeEquivalentTo(category);
    }

    [Fact]
    public async Task RemoveAsync()
    {
        var category = new Category()
        {
            Id = new Guid()
        };

        var bytes = JsonSerializer.SerializeToUtf8Bytes(category);

        await _cacheService.SetAsync(CACHEKEY, bytes);

        var returnCategory = await _cacheService.GetAsync<Category>(CACHEKEY);

        returnCategory.Should().NotBeNull();
        returnCategory!.Id.Should().Be(category.Id);

        await _cacheService.RemoveAsync(CACHEKEY);

        var emptyCacheCategory = await _cacheService.GetAsync<Category>(CACHEKEY);
        emptyCacheCategory.Should().BeNull();

    }

    [Fact]
    public async Task ExistsAsync()
    {
        var category = new Category()
        {
            Id = new Guid()
        };

        var bytes = JsonSerializer.SerializeToUtf8Bytes(category);

        await _cacheService.SetAsync(CACHEKEY, bytes);

        var returnCategory = await _cacheService.GetAsync<Category>(CACHEKEY);

        returnCategory.Should().NotBeNull();
        returnCategory!.Id.Should().Be(category.Id);

        var existCategory = await _cacheService.ExistsAsync(CACHEKEY);

        existCategory.Should().BeTrue();
    }
}
