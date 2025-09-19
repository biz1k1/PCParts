using AutoMapper;
using FluentAssertions;
using Moq;
using PCParts.Application.Abstraction.Storage;
using PCParts.Application.Command;
using PCParts.Application.Model.Enums;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.SpecificationService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;
using PCParts.Domain.Specification.Specification;
using PCParts.Storage.Mapping;

namespace PCParts.Application.Tests.Services;

public class SpecificationServiceShould
{
    private readonly Mock<ICategoryStorage> _categoryStorage;
    private readonly Mock<ISpecificationStorage> _specificationStorage;
    private readonly Mock<ISpecificationValueStorage> _specificationValueStorage;
    private readonly SpecificationService _sut;
    private readonly Mock<IValidationService> _validator;
    private readonly Mapper _mapper;

    public SpecificationServiceShould()
    {
        _categoryStorage = new Mock<ICategoryStorage>();
        _specificationStorage = new Mock<ISpecificationStorage>();
        _validator = new Mock<IValidationService>();
        _specificationValueStorage = new Mock<ISpecificationValueStorage>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new StorageProfile())));

        _sut = new SpecificationService(
            _specificationStorage.Object,
            _categoryStorage.Object,
            _specificationValueStorage.Object,
            _validator.Object,
            _mapper);
    }

    [Fact]
    public async Task ReturnCreatedSpecification()
    {
        var categoryId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

        var category = new Domain.Entities.Category()
        {
            Id = categoryId
        };

        var domainSpecification = new Domain.Entities.Specification
        {
            Name = "spec",
            DataType = Domain.Enums.SpecificationDataType.StringType,
        };

        var specificationCommand = new CreateSpecificationCommand(
            categoryId,
            "spec",
            SpecificationDataType.StringType);

        var applicationSpecification = _mapper.Map<Specification>(domainSpecification);

        var returnComponentSetup = _categoryStorage.Setup(x =>
                x.GetCategory(categoryId, null, It.IsAny<CancellationToken>()));
        returnComponentSetup.ReturnsAsync(category);
        var returnSpecification = _specificationStorage.Setup(x =>
            x.CreateSpecification(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Domain.Enums.SpecificationDataType>(), It.IsAny<CancellationToken>()));
        returnSpecification.ReturnsAsync(domainSpecification);

        var actual = await _sut.CreateSpecification(specificationCommand, It.IsAny<CancellationToken>());
        actual.Should().BeEquivalentTo(applicationSpecification);
        _specificationStorage.Verify(x =>
            x.CreateSpecification(categoryId, "spec", Domain.Enums.SpecificationDataType.StringType, CancellationToken.None), Times.Once);
        _categoryStorage.Verify(x =>
            x.GetCategory(categoryId, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReturnUpdatedSpecification()
    {
        var specificationId = Guid.Parse("333357ef-a6ed-40ec-ae5f-44383f00ca17");
        var specificationValueId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

        var command = new UpdateSpecificationCommand(specificationId, "text", SpecificationDataType.StringType);

        var domainSpecification = new Domain.Entities.Specification
        {
            Id = specificationId,
            Name = "spec",
            DataType = Domain.Enums.SpecificationDataType.StringType
        };

        var domainSpecificationValue = new Domain.Entities.SpecificationValue()
        {
            Id = specificationValueId,
            Value = "string"
        };

        var domainUpdatedSpecification = new Domain.Entities.Specification
        {
            Id = specificationId,
            Name = command.Name
        };

        var applicationUpdatedSpecification = _mapper.Map<Specification>(domainUpdatedSpecification);

        var getSpecificationSetup = _specificationStorage.Setup(x =>
            x.GetSpecification(It.IsAny<Guid>(), null, CancellationToken.None));
        getSpecificationSetup.ReturnsAsync(domainSpecification);
        var updateSpecificationSetup = _specificationStorage.Setup(x =>
            x.UpdateSpecification(It.IsAny<Domain.Entities.Specification>(), It.IsAny<Dictionary<string, object>>(), CancellationToken.None));
        updateSpecificationSetup.ReturnsAsync(domainUpdatedSpecification);
        var getSpecificationValueSetup = _specificationValueStorage.Setup(x =>
            x.GetSpecificationValue(It.IsAny<Guid>(), null, CancellationToken.None));
        getSpecificationValueSetup.ReturnsAsync(domainSpecificationValue);

        var actual = await _sut.UpdateSpecification(command, CancellationToken.None);
        actual.Should().BeEquivalentTo(applicationUpdatedSpecification);
        _specificationStorage.Verify(x => x.GetSpecification(specificationId, null, CancellationToken.None));
        _specificationStorage.Verify(x => x.UpdateSpecification(domainSpecification, It.IsAny<Dictionary<string, object>>(), CancellationToken.None), Times.Once);
        _specificationValueStorage.Verify(x => x.GetSpecificationValue(specificationId, null, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ThrowCategoryNotFoundException_WhenCreateSpecification_IfCategoryIsNull()
    {
        var categoryId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

        await _sut.Invoking(x =>
                x.CreateSpecification(new CreateSpecificationCommand(categoryId, null, SpecificationDataType.StringType), CancellationToken.None))
            .Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task ThrowComponentNotFoundException_WhenUpdateSpecification_IfSpecificationIsNull()
    {
        var specificationId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

        await _sut.Invoking(x =>
                x.UpdateSpecification(
                    new UpdateSpecificationCommand(specificationId, null, SpecificationDataType.StringType),
                    CancellationToken.None))
            .Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task ThrowSpecificationValueNotFoundException_WhenUpdateSpecification_IfSpecificationValueFromSpecificationIsNull()
    {
        var specificationId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

        var domainSpecification = new Domain.Entities.Specification()
        {
            Id = specificationId
        };

        var getSpecificationSetup = _specificationStorage.Setup(x =>
            x.GetSpecification(It.IsAny<Guid>(), null, CancellationToken.None));
        getSpecificationSetup.ReturnsAsync(domainSpecification);

        await _sut.Invoking(x =>
                x.UpdateSpecification(new UpdateSpecificationCommand(specificationId, null, SpecificationDataType.StringType), CancellationToken.None))
            .Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task ThrowInvalidSpecificationTypeException_WhenUpdateSpecification_IfCommandTypeInvalid()
    {
        var specificationId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

        var domainSpecification = new Domain.Entities.Specification()
        {
            Id = specificationId,
            SpecificationValues = new List<Domain.Entities.SpecificationValue>()

        };

        var getSpecificationSetup = _specificationStorage.Setup(x =>
            x.GetSpecification(It.IsAny<Guid>(), null, CancellationToken.None));
        getSpecificationSetup.ReturnsAsync(domainSpecification);

        await _sut.Invoking(x =>
                x.UpdateSpecification(new UpdateSpecificationCommand(specificationId, null, null), CancellationToken.None))
            .Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task ThrowCategoryNotFoundException_WhenGetSpecificationByCategory_IfSpecificationIsNull()
    {
        await _sut.Invoking(x =>
                x.GetSpecificationsByCategory(Guid.Empty, CancellationToken.None))
            .Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task ThrowSpecificationNotFoundException_WhenRemoveSpecification_IfSpecificationIsNull()
    {
        await _sut.Invoking(x =>
                x.RemoveSpecification(Guid.Empty, CancellationToken.None))
            .Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task ThrowRemoveEntityWithChildrenException_WhenRemoveSpecification_IfSpecificationValuesFromSpecificationIsNull()
    {
        var specificationId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
        var specification = new Domain.Entities.Specification()
        {
            Id = specificationId
        };

        var getSpecificationSetup = _specificationStorage.Setup(x =>
            x.GetSpecification(It.IsAny<Guid>(), It.IsAny<SpecificationWithSpecificationValueSpec>(), CancellationToken.None));
        getSpecificationSetup.ReturnsAsync(specification);

        await _sut.Invoking(x =>
                x.RemoveSpecification(specificationId, CancellationToken.None))
            .Should().ThrowAsync<RemoveEntityWithChildrenException>();
    }
}

