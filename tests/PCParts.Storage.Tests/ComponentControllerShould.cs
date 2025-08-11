using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PCParts.API.Model.Models;

namespace PCParts.API.Tests
{
    public class ComponentControllerShould : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly ApiWebApplicationFactory _factory;

        private const string CategoryName = "category";
        private const string ComponentName = "component";
        private const string SpecificationName = "Specficiation";
        public ComponentControllerShould(
            ApiWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateComponent()
        {
            using var httpClient = _factory.CreateClient();

            using var responseCategory = await httpClient.PostAsync("Categories",
                JsonContent.Create(new { Name = CategoryName }));
            responseCategory.StatusCode.Should().Be(HttpStatusCode.Created);

            var category = await responseCategory.Content.ReadFromJsonAsync<Category>();

            category
                .Should().NotBeNull().And
                .Subject.As<Category>().Name.Should().Be(CategoryName);


            using var responseSpecification = await httpClient.PostAsync("/Specifications",
                JsonContent.Create(new { Name = SpecificationName, Type = "StringType", CategoryId = category.Id }));
            responseSpecification.StatusCode.Should().Be(HttpStatusCode.Created);

            var specification = await responseSpecification.Content.ReadFromJsonAsync<SpecificationResponse>();
            specification
                .Should().NotBeNull().And
                .Subject.As<SpecificationResponse>().Name.Should().Be(SpecificationName);


            using var responseComponent = await httpClient.PostAsync("/Components",
                JsonContent.Create(new
                {
                    name = ComponentName,
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
                .Subject.As<Component>().Name.Should().Be(ComponentName);
        }
    }

    public class SpecificationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
