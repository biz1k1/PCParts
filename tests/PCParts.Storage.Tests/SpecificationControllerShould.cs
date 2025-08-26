using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using PCParts.API.Model.Models;
using PCParts.Application.Model.Enums;
namespace PCParts.API.Tests;

public class SpecificationControllerShould : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;

    private const string SampleCategoryName = "category";
    private const string SampleSpecificationName = "specification";
    private const string SampleUpdateSpecificationName = "newSpecificationName";

    public SpecificationControllerShould(
        ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateSpecification()
    {
        using var httpClient = _factory.CreateClient();

        using var responseCategoryCreate = await httpClient.PostAsync("/Categories",
            JsonContent.Create(new { Name = SampleCategoryName }));
        responseCategoryCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdCategory = await responseCategoryCreate.Content.ReadFromJsonAsync<Category>();
        createdCategory
            .Should().NotBeNull().And
            .Subject.As<Category>().Name.Should().Be(SampleCategoryName);

        using var responseSpecificationCreate = await httpClient.PostAsync("/Specifications",
            JsonContent.Create(new CreateSpecification()
            { Name = SampleSpecificationName, Type = SpecificationDataType.StringType, CategoryId = createdCategory.Id }));
        responseSpecificationCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdSpecification = await responseSpecificationCreate.Content.ReadFromJsonAsync<SpecificationResponse>();
        createdSpecification
            .Should().NotBeNull().And
            .Subject.As<SpecificationResponse>().Name.Should().Be(SampleSpecificationName);
    }

    [Fact]
    public async Task GetSpecificationsByCategory()
    {
        using var httpClient = _factory.CreateClient();

        using var responseCategoryCreate = await httpClient.PostAsync("/Categories",
            JsonContent.Create(new { Name = SampleCategoryName }));
        responseCategoryCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdCategory = await responseCategoryCreate.Content.ReadFromJsonAsync<Category>();
        createdCategory
            .Should().NotBeNull().And
            .Subject.As<Category>().Name.Should().Be(SampleCategoryName);

        using var responseSpecificationCreate = await httpClient.PostAsync("/Specifications",
            JsonContent.Create(new CreateSpecification()
            { Name = SampleSpecificationName, Type = SpecificationDataType.StringType, CategoryId = createdCategory.Id }));
        responseSpecificationCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdSpecification = await responseSpecificationCreate.Content.ReadFromJsonAsync<SpecificationResponse>();
        createdSpecification
            .Should().NotBeNull().And
            .Subject.As<SpecificationResponse>().Name.Should().Be(SampleSpecificationName);

        using var responseSpecificationGetByCategory = await httpClient.GetAsync($"/Specifications/{createdCategory.Id}");
        responseSpecificationGetByCategory.StatusCode.Should().Be(HttpStatusCode.OK);

        var getSpecificationByCategory = await responseSpecificationCreate.Content.ReadFromJsonAsync<SpecificationResponse>();
        getSpecificationByCategory
            .Should().NotBeNull().And
            .Subject.As<SpecificationResponse>().Name.Should().Be(SampleSpecificationName);
    }

    [Fact]
    public async Task UpdateSpecification()
    {
        using var httpClient = _factory.CreateClient();

        using var responseCategoryCreate = await httpClient.PostAsync("/Categories",
            JsonContent.Create(new { Name = SampleCategoryName }));
        responseCategoryCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdCategory = await responseCategoryCreate.Content.ReadFromJsonAsync<Category>();
        createdCategory
            .Should().NotBeNull().And
            .Subject.As<Category>().Name.Should().Be(SampleCategoryName);

        using var responseSpecificationCreate = await httpClient.PostAsync("/Specifications",
            JsonContent.Create(new CreateSpecification()
            { Name = SampleSpecificationName, Type = SpecificationDataType.StringType, CategoryId = createdCategory.Id }));
        responseSpecificationCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdSpecification = await responseSpecificationCreate.Content.ReadFromJsonAsync<SpecificationResponse>();
        createdSpecification
            .Should().NotBeNull().And
            .Subject.As<SpecificationResponse>().Name.Should().Be(SampleSpecificationName);


        using var responseSpecificationUpdate = await httpClient.PutAsync("/Specifications",
            JsonContent.Create(new UpdateSpecification
            { Name = SampleUpdateSpecificationName, Type = null, Id = createdCategory.Id }));
        responseSpecificationUpdate.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedSpecification = await responseSpecificationUpdate.Content.ReadFromJsonAsync<SpecificationResponse>();
        updatedSpecification
            .Should().NotBeNull().And
            .Subject.As<SpecificationResponse>().Name.Should().Be(SampleUpdateSpecificationName);
    }

    [Fact]
    public async Task RemoveSpecification()
    {

    }
    public class SpecificationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
