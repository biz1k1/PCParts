using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PCParts.API.Model.Models;

namespace PCParts.API.Tests;

public class CategoryControllerShould : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;
    private const string SampleCategoryName = "Category";
    private const string SampleUpdateCategoryName = "UpdateCategory";
    public CategoryControllerShould(
        ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCategories()
    {
        using var httpClient = _factory.CreateClient();

        using var response = await httpClient.GetAsync("/Categories");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
        categories.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCategory()
    {
        using var httpClient = _factory.CreateClient();

        using var responsePost = await httpClient.PostAsync("/Categories",
            JsonContent.Create(new { Name = SampleCategoryName }));
        responsePost.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdCategory = await responsePost.Content.ReadFromJsonAsync<Category>();

        using var responseGet = await httpClient.GetAsync($"{"/Categories"}/{createdCategory.Id}");
        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);

        var category = await responseGet.Content.ReadFromJsonAsync<Category>();
        category
            .Should().NotBeNull().And
            .Subject.As<Category>().Name.Should().Be(SampleCategoryName); ;

    }

    [Fact]
    public async Task CreateCategory()
    {
        using var httpClient = _factory.CreateClient();

        using var response = await httpClient.PostAsync("/Categories",
            JsonContent.Create(new { Name = SampleCategoryName }));
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var category = await response.Content.ReadFromJsonAsync<Category>();
        category
            .Should().NotBeNull().And
            .Subject.As<Category>().Name.Should().Be(SampleCategoryName);
    }

    [Fact]
    public async Task UpdateCategory()
    {
        using var httpClient = _factory.CreateClient();

        using var responsePost = await httpClient.PostAsync("/Categories",
            JsonContent.Create(new { Name = SampleCategoryName }));
        responsePost.StatusCode.Should().Be(HttpStatusCode.Created);

        var category = await responsePost.Content.ReadFromJsonAsync<Category>();
        category
            .Should().NotBeNull().And
            .Subject.As<Category>().Name.Should().Be(SampleCategoryName);

        using var responseUpdate = await httpClient.PutAsync("/Categories",
            JsonContent.Create(new { Id = category.Id, Name = SampleUpdateCategoryName }));
        responseUpdate.IsSuccessStatusCode.Should().BeTrue();

        var updatedCategory = await responseUpdate.Content.ReadFromJsonAsync<Category>();
        updatedCategory
            .Should().NotBeNull().And
            .Subject.As<Category>().Name.Should().Be(SampleUpdateCategoryName);
    }

    [Fact]
    public async Task RemoveCategory()
    {
        using var httpClient = _factory.CreateClient();

        using var responsePost = await httpClient.PostAsync("/Categories",
            JsonContent.Create(new { Name = SampleCategoryName }));
        responsePost.StatusCode.Should().Be(HttpStatusCode.Created);

        var category = await responsePost.Content.ReadFromJsonAsync<Category>();
        category
            .Should().NotBeNull().And
            .Subject.As<Category>().Name.Should().Be(SampleCategoryName);

        using var responseRemove = await httpClient.DeleteAsync($"{"/Categories"}/{category.Id}");
        responseRemove.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
