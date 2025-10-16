using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PCParts.API.Model.Models;
using PCParts.Application.Model.Enums;

namespace PCParts.API.Tests;

public class ComponentControllerShould : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;

    private const string SampleCategoryName = "category";
    private const string SampleComponentName = "component";
    private const string SampleComponentName2 = "component2";
    private const string SampleUpdateCategoryName = "newComponentName";
    private const string SampleSpecificationName = "Specficiation";

    public ComponentControllerShould(
        ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateComponent()
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
            .Subject.As<Component>().Category.Should().Be(SampleComponentName);
    }

    [Fact]
    public async Task GetComponent()
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
            JsonContent.Create(new CreateComponent()
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

        var createdComponent = await responseComponentCreate.Content.ReadFromJsonAsync<Component>();

        using var responseComponentGet = await httpClient.GetAsync($"{"Components"}/{createdComponent.Id}");
        responseComponentGet.StatusCode.Should().Be(HttpStatusCode.OK);

        var component = await responseComponentGet.Content.ReadFromJsonAsync<Component>();
        component
            .Should().NotBeNull().And
            .Subject.As<Component>().Category.Should().Be(SampleComponentName);
    }

    [Fact]
    public async Task GetComponents()
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
            JsonContent.Create(new CreateComponent()
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
        using var responseComponentCreate2 = await httpClient.PostAsync("/Components",
            JsonContent.Create(new CreateComponent()
            {
                Name = SampleComponentName2,
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
        responseComponentCreate2.StatusCode.Should().Be(HttpStatusCode.Created);

        using var responseComponentGet = await httpClient.GetAsync($"{"Components"}");
        responseComponentGet.StatusCode.Should().Be(HttpStatusCode.OK);

        var components = await responseComponentGet.Content.ReadFromJsonAsync<List<Component>>();
        components
            .Should().NotBeNull().And
            .Subject.As<List<Component>>();
    }

    [Fact]
    public async Task RemoveComponent()
    {
        using var httpClient = _factory.CreateClient();

        using var responseCategory = await httpClient.PostAsync("Categories",
            JsonContent.Create(new { Name = SampleCategoryName }));
        responseCategory.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdCategory = await responseCategory.Content.ReadFromJsonAsync<Category>();

        createdCategory
            .Should().NotBeNull().And
            .Subject.As<Category>().Name.Should().Be(SampleCategoryName);



        using var responseSpecificationCreate = await httpClient.PostAsync("/Specifications",
            JsonContent.Create(new CreateSpecification()
            { Name = SampleSpecificationName, Type = SpecificationDataType.StringType, CategoryId = createdCategory.Id }));
        responseSpecificationCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var specification = await responseSpecificationCreate.Content.ReadFromJsonAsync<SpecificationResponse>();
        specification
            .Should().NotBeNull().And
            .Subject.As<SpecificationResponse>().Name.Should().Be(SampleSpecificationName);


        using var responseComponentCreate = await httpClient.PostAsync("/Components",
            JsonContent.Create(new CreateComponent()
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

        var createdComponent = await responseComponentCreate.Content.ReadFromJsonAsync<Component>();

        using var responseComponentDelete = await httpClient.DeleteAsync($"{"/Components"}/{createdComponent.Id}");
        responseComponentDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);

    }

    [Fact]
    public async Task UpdateComponents()
    {
        using var httpClient = _factory.CreateClient();

        using var responseCategory = await httpClient.PostAsync("Categories",
            JsonContent.Create(new { Name = SampleCategoryName }));
        responseCategory.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdCategory = await responseCategory.Content.ReadFromJsonAsync<Category>();

        createdCategory
            .Should().NotBeNull().And
            .Subject.As<Category>().Name.Should().Be(SampleCategoryName);


        using var responseSpecificationCreate = await httpClient.PostAsync("/Specifications",
            JsonContent.Create(new CreateSpecification()
            { Name = SampleSpecificationName, Type = SpecificationDataType.StringType, CategoryId = createdCategory.Id }));
        responseSpecificationCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var specification = await responseSpecificationCreate.Content.ReadFromJsonAsync<SpecificationResponse>();
        specification
            .Should().NotBeNull().And
            .Subject.As<SpecificationResponse>().Name.Should().Be(SampleSpecificationName);


        using var responseComponentCreate = await httpClient.PostAsync("/Components",
            JsonContent.Create(new CreateComponent()
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

        using var responseComponentUpdate = await httpClient.PutAsync("/Components",
            JsonContent.Create(new UpdateComponent
            {
                Id = component.Id,
                Name = SampleUpdateCategoryName
            }));
        responseComponentUpdate.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedComponent = await responseComponentUpdate.Content.ReadFromJsonAsync<Component>();
        updatedComponent
            .Should().NotBeNull().And
            .Subject.As<Component>().Category.Should().Be(SampleUpdateCategoryName);


    }

    public class SpecificationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
