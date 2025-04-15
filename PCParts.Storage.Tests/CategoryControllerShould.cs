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
        using var httpClient = _factory.CreateClient();

        using var response = await httpClient.GetAsync("/categories");

        response.Invoking(x => x.EnsureSuccessStatusCode()).Should().NotThrow();

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be("[]");
    }

    [Fact]
    public async Task Database_Should_BeAvailable()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PgContext>();

        var canConnect = await db.Database.CanConnectAsync();
        canConnect.Should().BeTrue();
    }
}