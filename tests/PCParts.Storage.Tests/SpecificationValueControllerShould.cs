using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using PCParts.API.Model.Models;
using PCParts.Application.Model.Enums;

namespace PCParts.API.Tests;

public class SpecificationValueControllerShould : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;


    private const string SampleCategoryName = "category";
    private const string SampleComponentName = "component";
    private const string SampleSpecificationName = "Specficiation";
    private const string SampleSpecificationValueObj = "Name2";
    public SpecificationValueControllerShould(
        ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task UpdateSpecification()
    {
        using var httpClient = _factory.CreateClient();

        using var responseCategoryCreate = await httpClient.PostAsync("Categories",
            JsonContent.Create(new { Name = SampleCategoryName }));
        responseCategoryCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdCategory = await responseCategoryCreate.Content.ReadFromJsonAsync<Category>();

        createdCategory
            .Should().NotBeNull().And
            .Subject.As<Category>().Name.Should().Be(SampleCategoryName);


        using var responseSpecificationCreate = await httpClient.PostAsync("/Specifications",
            JsonContent.Create(new CreateSpecification
                { Name = SampleSpecificationName, Type = SpecificationDataType.StringType, CategoryId = createdCategory.Id }));
        responseSpecificationCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var specification = await responseSpecificationCreate.Content.ReadFromJsonAsync<SpecificationResponse>();
        specification
            .Should().NotBeNull().And
            .Subject.As<SpecificationResponse>().Name.Should().Be(SampleSpecificationName);


        using var responseComponentCreate = await httpClient.PostAsync("/Components",
            JsonContent.Create(new CreateComponent
            {
                Name = SampleComponentName,
                CategoryId = createdCategory.Id,
                SpecificationValues = new List<CreateSpecificationValue>()
                {
                    new()
                    {
                        SpecificationId = specification.Id,
                        Value = "valueString"
                    }
                }
            }));

        responseComponentCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var component = await responseComponentCreate.Content.ReadFromJsonAsync<Component>();
        component
            .Should().NotBeNull().And
            .Subject.As<Component>().Name.Should().Be(SampleComponentName);

        using var responseSpecificationValueUpdate = await httpClient.PatchAsync("/SpecificationsValues",
            JsonContent.Create(new UpdateSpecificationValue
            { SpecificationValueId = component.SpecificationValues
                    .Where(x=>x.SpecificationName == SampleSpecificationName).First().Id, 
                Value = SampleSpecificationValueObj}));
        responseSpecificationValueUpdate.StatusCode.Should().Be(HttpStatusCode.OK);

        var specificationValues = await responseSpecificationValueUpdate.Content.ReadFromJsonAsync<SpecificationValue>();
        ((JsonElement)specificationValues
                .As<SpecificationValue>()
                .Value)
            .GetString()
            .Should()
            .Be(SampleSpecificationValueObj);
    }

    public class SpecificationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
