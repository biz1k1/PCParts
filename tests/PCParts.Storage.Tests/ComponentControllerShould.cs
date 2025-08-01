using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PCParts.API.Model.Models;

namespace PCParts.API.Tests
{
    public class ComponentControllerShould : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly ApiWebApplicationFactory _factory;

        public ComponentControllerShould(
            ApiWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateComponent()
        {
            const string categoryName = "category";
            const string componentName = "component";
            const string specificationName = "Specficiation";

            using var httpClient = _factory.CreateClient();

            using var responseCategory = await httpClient.PostAsync("/Categories",
                JsonContent.Create(new { Name = categoryName }));
            responseCategory.StatusCode.Should().Be(HttpStatusCode.Created);

            var category = await responseCategory.Content.ReadFromJsonAsync<Category>();

            category
                .Should().NotBeNull().And
                .Subject.As<Category>().Name.Should().Be(categoryName);


            using var responseSpecification = await httpClient.PostAsync("/Specifications",
                JsonContent.Create(new { Name = specificationName, Type = "STRING", CategoryId = category.Id }));
            responseSpecification.StatusCode.Should().Be(HttpStatusCode.Created);

            var specification = await responseSpecification.Content.ReadFromJsonAsync<SpecificationResponse>();
            specification
                .Should().NotBeNull().And
                .Subject.As<SpecificationResponse>().Name.Should().Be(specificationName);


            using var responseComponent = await httpClient.PostAsync("/Components",
                JsonContent.Create(new
                {
                    name = componentName,
                    categoryId = category.Id,
                    specificationValues = new object[]
                    {
                        new
                        {
                            specificationId = specification.Id,
                            value = "valueString"
                        }
                    }
                }));

            responseComponent.StatusCode.Should().Be(HttpStatusCode.Created);

            var component = await responseComponent.Content.ReadFromJsonAsync<Component>();
            component
                .Should().NotBeNull().And
                .Subject.As<Component>().Name.Should().Be(componentName);
        }
    }
}

public class SpecificationResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
}