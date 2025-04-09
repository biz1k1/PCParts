using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace PCParts.Storage.Tests;

public class CategoryControllerShould : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;
    private readonly ITestOutputHelper _output;

    public CategoryControllerShould(
        ApiWebApplicationFactory factory,
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