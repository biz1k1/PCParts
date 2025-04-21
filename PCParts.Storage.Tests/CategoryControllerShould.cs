using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace PCParts.Storage.Tests;

public class CategoryControllerShould : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;
    public CategoryControllerShould(
        ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCategory()
    {
        const string categoryName = "category";

        using var httpClient = _factory.CreateClient();

        using var response = await httpClient.PostAsync("/Categories",
            JsonContent.Create(new { Name = categoryName }));
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var category = await response.Content.ReadFromJsonAsync<API.Model.Models.Category>();
        category
            .Should().NotBeNull().And
            .Subject.As<API.Model.Models.Category>().Name.Should().Be(categoryName);

        using var responseGet = await httpClient.GetAsync($"/Categories?id={category.Id}");
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task CreateCategory()
    {
        const string categoryName = "category";

        using var httpClient = _factory.CreateClient();

        using var response = await httpClient.PostAsync("/Categories",
            JsonContent.Create(new {Name= categoryName }));
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var category = await response.Content.ReadFromJsonAsync<API.Model.Models.Category>();
        category
            .Should().NotBeNull().And
            .Subject.As<API.Model.Models.Category>().Name.Should().Be(categoryName);
    }

    [Fact]
    public async Task UpdateCategory()
    {
        const string categoryName = "category";
        const string updateCategoryName = "updateCategory";

        using var httpClient = _factory.CreateClient();

        using var responsePost = await httpClient.PostAsync("/Categories",
            JsonContent.Create(new { Name = categoryName }));
        responsePost.StatusCode.Should().Be(HttpStatusCode.Created);

        var category = await responsePost.Content.ReadFromJsonAsync<API.Model.Models.Category>();
        category
            .Should().NotBeNull().And
            .Subject.As<API.Model.Models.Category>().Name.Should().Be(categoryName);

        using var responseUpdate= await httpClient.PutAsync("/Categories",
            JsonContent.Create(new {Id=category.Id ,Name = updateCategoryName }));
        responsePost.IsSuccessStatusCode.Should().BeTrue();

        var updatedCategory = await responseUpdate.Content.ReadFromJsonAsync<API.Model.Models.Category>();
        updatedCategory
            .Should().NotBeNull().And
            .Subject.As<API.Model.Models.Category>().Name.Should().Be(updateCategoryName);
    }

    [Fact]
    public async Task RemoveCategory()
    {
        const string categoryName = "category";

        using var httpClient = _factory.CreateClient();

        using var responsePost = await httpClient.PostAsync("/Categories",
            JsonContent.Create(new { Name = categoryName }));
        responsePost.StatusCode.Should().Be(HttpStatusCode.Created);

        var category = await responsePost.Content.ReadFromJsonAsync<API.Model.Models.Category>();
        category
            .Should().NotBeNull().And
            .Subject.As<API.Model.Models.Category>().Name.Should().Be(categoryName);

        using var responseRemove= await httpClient.DeleteAsync($"/Categories/{category.Id}");
        responseRemove.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}