using FluentAssertions;
using Moq;
using PCParts.Application.AbstractionStorage;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Enum;
using PCParts.Application.Model.Models;
using PCParts.Application.Model.QueryModel;
using PCParts.Application.Services.QueryBuilderService;
using PCParts.Application.Services.SpecificationService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Tests.Services;

public class SpecificationServiceShould
{
    private readonly Mock<ICategoryStorage> _categoryStorage;
    private readonly Mock<IComponentStorage> _componentStorage;
    private readonly Mock<IQueryBuilderService> _queryBuilder;
    private readonly Mock<ISpecificationStorage> _storageSpecification;
    private readonly Mock<ISpecificationValueStorage> _specificationValueStorage;
    private readonly ISpecificationService _sut;
    private readonly Mock<IValidationService> _validator;

    public SpecificationServiceShould()
    {
        _categoryStorage = new Mock<ICategoryStorage>();
        _componentStorage = new Mock<IComponentStorage>();
        _storageSpecification = new Mock<ISpecificationStorage>();
        _queryBuilder = new Mock<IQueryBuilderService>();
        _validator = new Mock<IValidationService>();
        _specificationValueStorage = new Mock<ISpecificationValueStorage>();

        _sut = new SpecificationService(
            _storageSpecification.Object,
            _categoryStorage.Object,
            _specificationValueStorage.Object,
            _validator.Object,
            _queryBuilder.Object);
    }

    //[Fact]
    //public async Task ReturnCreatedSpecification()
    //{
    //    var componentId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
    //    var specificationId = Guid.Parse("333357ef-a6ed-40ec-ae5f-44383f00ca17");
    //    var component = new Component
    //    {
    //        Id = componentId
    //    };
    //    var specification = new Specification
    //    {
    //        Name = "spec",
    //        Type = SpecificationDataType.STRING,
    //        Value = "text"
    //    };
    //    var specificationCommand = new CreateSpecificationCommand(
    //        componentId,
    //        "spec",
    //        "text",
    //        SpecificationDataType.STRING);

    //    var returnComponentSetup = _storageComponent.Setup(x =>
    //            x.GetComponent(componentId, It.IsAny<string[]>(), It.IsAny<CancellationToken>()));
    //    returnComponentSetup.ReturnsAsync(component);
    //    var returnSpecification = _storageSpecification.Setup(x =>
    //        x.CreateSpecification(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<SpecificationDataType>(), It.IsAny<CancellationToken>()));
    //    returnSpecification.ReturnsAsync(specification);

    //    var actual = await _sut.CreateSpecification(specificationCommand, It.IsAny<CancellationToken>());
    //    actual.Should().BeSameAs(specification);
    //    _storageSpecification.Verify(x =>
    //        x.CreateSpecification(componentId, "spec", SpecificationDataType.STRING, CancellationToken.None), Times.Once);
    //    _storageComponent.Verify(x =>
    //        x.GetComponent(componentId, null, It.IsAny<CancellationToken>()), Times.Once);
    //}

    [Fact]
    public async Task ReturnUpdatedSpecification()
    {
        var specificationId = Guid.Parse("333357ef-a6ed-40ec-ae5f-44383f00ca17");
        var command = new UpdateSpecificationCommand(specificationId, "text", SpecificationDataType.STRING);
        var query = new UpdateQuery { Id = specificationId };
        var specification = new Specification
        {
            Id = specificationId,
            Name = "spec",
            Type = SpecificationDataType.STRING
        };
        var updatedSpecification = new Specification
        {
            Id = specificationId,
            Name = command.Name
        };

        var buildQuerySetup = _queryBuilder.Setup(x =>
            x.BuildSpecificationUpdateQuery(It.IsAny<UpdateSpecificationCommand>()));
        buildQuerySetup.Returns(query);
        var getSpecificationSetup = _storageSpecification.Setup(x =>
            x.GetSpecification(It.IsAny<Guid>(),null, CancellationToken.None));
        getSpecificationSetup.ReturnsAsync(specification);
        var updateSpecificationSetup = _storageSpecification.Setup(x =>
            x.UpdateSpecification(It.IsAny<UpdateQuery>(), CancellationToken.None));
        updateSpecificationSetup.ReturnsAsync(updatedSpecification);

        var actual = await _sut.UpdateSpecification(command, CancellationToken.None);
        actual.Should().Be(updatedSpecification);
        _storageSpecification.Verify(x => x.GetSpecification(specificationId, null, CancellationToken.None));
        _storageSpecification.Verify(x => x.UpdateSpecification(query, CancellationToken.None), Times.Once);
    }
    //[Fact]
    //public async Task ThrowComponentNotFoundException_WhenCreateSpecification_IfComponentIsNull()
    //{
    //    var componentId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

    //    await _sut.Invoking(x =>
    //            x.CreateSpecification(new CreateSpecificationCommand(componentId, null, null, SpecificationDataType.STRING),CancellationToken.None))
    //        .Should().ThrowAsync<ComponentNotFoundException>();
    //}

    [Fact]
    public async Task ThrowComponentNotFoundException_WhenUpdateSpecification_IfSpecificationIsNull()
    {
        var specificationId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

        await _sut.Invoking(x =>
                x.UpdateSpecification(
                    new UpdateSpecificationCommand(specificationId, null, SpecificationDataType.STRING),
                    CancellationToken.None))
            .Should().ThrowAsync<SpecificationNotFoundException>();
    }
}