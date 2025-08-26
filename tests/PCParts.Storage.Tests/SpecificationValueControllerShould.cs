namespace PCParts.API.Tests;

public class SpecificationValueControllerShould : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;

    public SpecificationValueControllerShould(
        ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateSpecification()
    {
        using var httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task GetSpecificationsByCategory()
    {

    }

    [Fact]
    public async Task UpdateSpecification()
    {

    }

    [Fact]
    public async Task RemoveSpecification()
    {

    }
}
