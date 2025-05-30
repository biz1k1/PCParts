//using FluentAssertions;
//using Moq;
//using PCParts.Application.Abstraction;
//using PCParts.Application.Command;
//using PCParts.Application.Model.Enum;
//using PCParts.Application.Model.Models;
//using PCParts.Application.Model.QueryModel;
//using PCParts.Application.Services.QueryBuilderService;
//using PCParts.Application.Services.SpecificationService;
//using PCParts.Application.Services.ValidationService;
//using PCParts.Application.Storages;
//using PCParts.Domain.Exceptions;

//namespace PCParts.Application.Tests.Services;

//public class SpecificationServiceShould
//{
//    private readonly Mock<ICategoryStorage> _categoryStorage;
//    private readonly Mock<IQueryBuilderService> _queryBuilder;
//    private readonly Mock<ISpecificationStorage> _specificationStorage;
//    private readonly Mock<ISpecificationValueStorage> _specificationValueStorage;
//    private readonly ISpecificationService _sut;
//    private readonly Mock<IValidationService> _validator;

//    public SpecificationServiceShould()
//    {
//        _categoryStorage = new Mock<ICategoryStorage>();
//        _specificationStorage = new Mock<ISpecificationStorage>();
//        _queryBuilder = new Mock<IQueryBuilderService>();
//        _validator = new Mock<IValidationService>();
//        _specificationValueStorage = new Mock<ISpecificationValueStorage>();

//        _sut = new SpecificationService(
//            _specificationStorage.Object,
//            _categoryStorage.Object,
//            _specificationValueStorage.Object,
//            _validator.Object,
//            _queryBuilder.Object);
//    }

//    [Fact]
//    public async Task ReturnCreatedSpecification()
//    {
//        var categoryId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
//        var specificationId = Guid.Parse("333357ef-a6ed-40ec-ae5f-44383f00ca17");
//        var component = new Category()
//        {
//            Id = categoryId
//        };
//        var specification = new Specification
//        {
//            Name = "spec",
//            Type = SpecificationDataType.STRING,
//        };
//        var specificationCommand = new CreateSpecificationCommand(
//            categoryId,
//            "spec",
//            SpecificationDataType.STRING);

//        var returnComponentSetup = _categoryStorage.Setup(x =>
//                x.GetCategory(categoryId, null, It.IsAny<CancellationToken>()));
//        returnComponentSetup.ReturnsAsync(component);
//        var returnSpecification = _specificationStorage.Setup(x =>
//            x.CreateSpecification(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<SpecificationDataType>(), It.IsAny<CancellationToken>()));
//        returnSpecification.ReturnsAsync(specification);

//        var actual = await _sut.CreateSpecification(specificationCommand, It.IsAny<CancellationToken>());
//        actual.Should().BeSameAs(specification);
//        _specificationStorage.Verify(x =>
//            x.CreateSpecification(categoryId, "spec", SpecificationDataType.STRING, CancellationToken.None), Times.Once);
//        _categoryStorage.Verify(x =>
//            x.GetCategory(categoryId, null, It.IsAny<CancellationToken>()), Times.Once);
//    }

//    [Fact]
//    public async Task ReturnUpdatedSpecification()
//    {
//        var specificationId = Guid.Parse("333357ef-a6ed-40ec-ae5f-44383f00ca17");
//        var specificationValueId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
//        var command = new UpdateSpecificationCommand(specificationId, "text", SpecificationDataType.STRING);
//        var query = new UpdateQuery { Id = specificationId };
//        var specification = new Specification
//        {
//            Id = specificationId,
//            Name = "spec",
//            Type = SpecificationDataType.STRING
//        };
//        var specificationValue = new SpecificationValue()
//        {
//            Id = specificationValueId,
//            Value = "string"
//        };
//        var updatedSpecification = new Specification
//        {
//            Id = specificationId,
//            Name = command.Name
//        };

//        var buildQuerySetup = _queryBuilder.Setup(x =>
//            x.BuildSpecificationUpdateQuery(It.IsAny<UpdateSpecificationCommand>()));
//        buildQuerySetup.Returns(query);
//        var getSpecificationSetup = _specificationStorage.Setup(x =>
//            x.GetSpecification(It.IsAny<Guid>(),null, CancellationToken.None));
//        getSpecificationSetup.ReturnsAsync(specification);
//        var updateSpecificationSetup = _specificationStorage.Setup(x =>
//            x.UpdateSpecification(It.IsAny<UpdateQuery>(), CancellationToken.None));
//        updateSpecificationSetup.ReturnsAsync(updatedSpecification);
//        var getSpecificationValueSetup = _specificationValueStorage.Setup(x =>
//            x.GetSpecificationValue(It.IsAny<Guid>(), null, CancellationToken.None));
//        getSpecificationValueSetup.ReturnsAsync(specificationValue);

//        var actual = await _sut.UpdateSpecification(command, CancellationToken.None);
//        actual.Should().Be(updatedSpecification);
//        _specificationStorage.Verify(x => x.GetSpecification(specificationId, null, CancellationToken.None));
//        _specificationStorage.Verify(x => x.UpdateSpecification(query, CancellationToken.None), Times.Once);
//        _specificationValueStorage.Verify(x => x.GetSpecificationValue(specificationId, null, CancellationToken.None), Times.Once);
//    }

//    [Fact]
//    public async Task ThrowCategoryNotFoundException_WhenCreateSpecification_IfCategoryIsNull()
//    {
//        var categoryId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

//        await _sut.Invoking(x =>
//                x.CreateSpecification(new CreateSpecificationCommand(categoryId, null, SpecificationDataType.STRING), CancellationToken.None))
//            .Should().ThrowAsync<EntityNotFoundException>();
//    }

//    [Fact]
//    public async Task ThrowComponentNotFoundException_WhenUpdateSpecification_IfSpecificationIsNull()
//    {
//        var specificationId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

//        await _sut.Invoking(x =>
//                x.UpdateSpecification(
//                    new UpdateSpecificationCommand(specificationId, null, SpecificationDataType.STRING),
//                    CancellationToken.None))
//            .Should().ThrowAsync<EntityNotFoundException>();
//    }

//    [Fact]
//    public async Task ThrowSpecificationValueNotFoundException_WhenUpdateSpecification_IfSpecificationValueFromSpecificationIsNull()
//    {
//        var specificationId= Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
//        var specification = new Specification()
//        {
//            Id=specificationId
//        };

//        var getSpecificationSetup = _specificationStorage.Setup(x =>
//            x.GetSpecification(It.IsAny<Guid>(), null, CancellationToken.None));
//        getSpecificationSetup.ReturnsAsync(specification);

//        await _sut.Invoking(x =>
//                x.UpdateSpecification(new UpdateSpecificationCommand(specificationId, null, SpecificationDataType.STRING), CancellationToken.None))
//            .Should().ThrowAsync<EntityNotFoundException>();
//    }

//    [Fact]
//    public async Task ThrowInvalidSpecificationTypeException_WhenUpdateSpecification_IfCommandTypeInvalid()
//    {
//        var specificationId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
//        var specification = new Specification()
//        {
//            Id = specificationId,
//            SpecificationValues = new List<SpecificationValue>()
            
//        };

//        var getSpecificationSetup = _specificationStorage.Setup(x =>
//            x.GetSpecification(It.IsAny<Guid>(), null, CancellationToken.None));
//        getSpecificationSetup.ReturnsAsync(specification);

//        await _sut.Invoking(x =>
//                x.UpdateSpecification(new UpdateSpecificationCommand(specificationId, null, null), CancellationToken.None))
//            .Should().ThrowAsync<EntityNotFoundException>();
//    }

//    [Fact]
//    public async Task ThrowCategoryNotFoundException_WhenGetSpecificationByCategory_IfSpecificationIsNull()
//    {
//        await _sut.Invoking(x =>
//                x.GetSpecificationsByCategory(Guid.Empty, CancellationToken.None))
//            .Should().ThrowAsync<EntityNotFoundException>();
//    }

//    [Fact]
//    public async Task ThrowSpecificationNotFoundException_WhenRemoveSpecification_IfSpecificationIsNull()
//    {
//        await _sut.Invoking(x =>
//                x.RemoveSpecification(Guid.Empty, CancellationToken.None))
//            .Should().ThrowAsync<EntityNotFoundException>();
//    }

//    [Fact]
//    public async Task ThrowRemoveEntityWithChildrenException_WhenRemoveSpecification_IfSpecificationValuesFromSpecificationIsNull()
//    {
//        var specificationId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
//        var specification = new Specification()
//        {
//            Id = specificationId
//        };

//        var getSpecificationSetup = _specificationStorage.Setup(x =>
//            x.GetSpecification(It.IsAny<Guid>(), It.IsAny<string[]>(), CancellationToken.None));
//        getSpecificationSetup.ReturnsAsync(specification);

//        await _sut.Invoking(x =>
//                x.RemoveSpecification(specificationId, CancellationToken.None))
//            .Should().ThrowAsync<RemoveEntityWithChildrenException>();
//    }
//}