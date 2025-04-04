using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace PCParts.Storage.Tests;

public class CategoryControllerShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public CategoryControllerShould(
        WebApplicationFactory<Program> factory,
        ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
    }

    [Fact]
    public async Task GetCategory()
    {
        using var httpClient = _factory.CreateClient();

        using var response = await httpClient.GetAsync("/categories");

        response.Invoking(x => x.EnsureSuccessStatusCode()).Should().NotThrow();

        var result = await response.Content.ReadAsStringAsync();

        //result.Should().Be("[]");
    }
}