//using FluentAssertions;
//using Moq;
//using PCParts.Application.Abstraction;
//using PCParts.Application.Command;
//using PCParts.Application.Model.Models;
//using PCParts.Application.Model.QueryModel;
//using PCParts.Application.Services.ComponentService;
//using PCParts.Application.Services.QueryBuilderService;
//using PCParts.Application.Services.SpecificationService;
//using PCParts.Application.Services.SpecificationValueService;
//using PCParts.Application.Services.ValidationService;
//using PCParts.Application.Storages;
//using PCParts.Domain.Exceptions;

//namespace PCParts.Application.Tests.Services;

//public class ComponentServiceShould
//{
//    private readonly Mock<IQueryBuilderService> _queryBuilder;
//    private readonly Mock<ICategoryStorage> _storageCategory;
//    private readonly Mock<IComponentStorage> _storageComponent;
//    private readonly Mock<ISpecificationValueService> _specificationValueService;
//    private readonly Mock<ISpecificationService> _specificationService;
//    private readonly IComponentService _sut;
//    private readonly Mock<IValidationService> _validator;
//    private readonly Mock<IUnitOfWork> _unitOfWork;

//    public ComponentServiceShould()
//    {
//        _storageComponent = new Mock<IComponentStorage>();
//        _storageCategory = new Mock<ICategoryStorage>();
//        _validator = new Mock<IValidationService>();
//        _queryBuilder = new Mock<IQueryBuilderService>();
//        _specificationService = new Mock<ISpecificationService>();
//        _specificationValueService = new Mock<ISpecificationValueService>();
//        _unitOfWork = new Mock<IUnitOfWork>();


//        _sut = new ComponentService(
//            _storageComponent.Object,
//            _storageCategory.Object,
//            _validator.Object,
//            _queryBuilder.Object,
//            _specificationValueService.Object,
//            _specificationService.Object,
//            _unitOfWork.Object);
//    }

//    [Fact]
//    public async Task ReturnComponents_FromStorage()
//    {
//        var components = new Component[]
//        {
//            new() { Id = Guid.Parse("c53de6bc-571e-43d2-84b8-13c99c676917") },
//            new() { Id = Guid.Parse("d8c4180f-cccc-4c93-8352-448079b77cd0") }
//        };

//        var returnComponentSetup = _storageComponent.Setup(x =>
//            x.GetComponents(It.IsAny<CancellationToken>()));
//        returnComponentSetup.ReturnsAsync(components);

//        var actual = await _sut.GetComponents(CancellationToken.None);
//        actual.Should().BeSameAs(components);
//        _storageComponent.Verify(x => x.GetComponents(CancellationToken.None), Times.Once);
//    }

//    [Fact]
//    public async Task ReturnComponent_FromStorage()
//    {
//        var componentId = Guid.Parse("86bc4fa7-7c86-4685-9131-8436220a8dba");
//        string[] includes = { "Specification", "Category" };
//        var component = new Component
//        {
//            Id = componentId
//        };

//        var returnComponentSetup = _storageComponent.Setup(x =>
//            x.GetComponent(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
//        returnComponentSetup.ReturnsAsync(component);

//        var actual = await _sut.GetComponent(componentId, CancellationToken.None);
//        actual.Should().BeSameAs(component);
//        _storageComponent.Verify(x => x.GetComponent(componentId, CancellationToken.None), Times.Once);
//    }

//    [Fact]
//    public async Task ReturnCreatedComponent()
//    {
//        var specificationId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
//        var componentId = Guid.Parse("333357ef-a6ed-40ec-ae5f-44383f00ca17");
//        var categoryId = Guid.Parse("d8c4180f-cccc-4c93-8352-448079b77cd0");
//        List<Specification> specifications = new List<Specification>()
//        {
//            new Specification(){Id=specificationId,SpecificationValues = new List<SpecificationValue>()
//            {
//                new SpecificationValue(){Id=specificationId}
//            }}
//        };
//        var command = new CreateComponentCommand("Component", categoryId, new List<CreateSpecificationValueCommand>()
//        {
//            new CreateSpecificationValueCommand(specificationId, null)
//        });
//        var component = new Component
//        {
//            Id = componentId,
//            Name = "Component",
//            SpecificationValues= new List<SpecificationValue>()
//            {
//                new SpecificationValue{Id=specificationId, Value = null,SpecificationName = null, Specification = null}
//            }
//        };

//        var returnCategorySetup = _specificationService.Setup(x =>
//            x.GetSpecificationsByCategory(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
//        returnCategorySetup.ReturnsAsync(specifications);
//        var returnComponentSetup = _storageComponent.Setup(x =>
//            x.CreateComponent(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
//        returnComponentSetup.ReturnsAsync(component);


//        var actual = await _sut.CreateComponent(command, CancellationToken.None);
//        actual.Should().BeSameAs(component);
//        _storageComponent.Verify(x => x.CreateComponent("Component", categoryId, CancellationToken.None), Times.Once);
//        _specificationService.Verify(x=>x.GetSpecificationsByCategory(categoryId,CancellationToken.None));
//    }

//    [Fact]
//    public async Task ReturnUpdatedComponent()
//    {
//        var category = new Category { Id = Guid.Parse("86bc4fa7-7c86-4685-9131-8436220a8dba") };
//        var component = new Component
//            { Id = Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a"), Name = "name", Category = category };
//        var command = new UpdateComponentCommand(component.Id, null, category.Id);
//        var query = new UpdateQuery();

//        var updateComponentSetup = _storageComponent.Setup(x =>
//            x.UpdateComponent(It.IsAny<UpdateQuery>(), It.IsAny<CancellationToken>()));
//        updateComponentSetup.ReturnsAsync(component);

//        var getCategorySetup = _storageCategory.Setup(x =>
//            x.GetCategory(It.IsAny<Guid>(), null, CancellationToken.None));
//        getCategorySetup.ReturnsAsync(category);

//        var getComponentSetup = _storageComponent.Setup(x =>
//            x.GetComponent(It.IsAny<Guid>(), CancellationToken.None));
//        getComponentSetup.ReturnsAsync(component);

//        var getQuerySetup = _queryBuilder.Setup(x =>
//            x.BuildComponentUpdateQuery(It.IsAny<UpdateComponentCommand>()));
//        getQuerySetup.Returns(query);

//        var actual = await _sut.UpdateComponent(command, CancellationToken.None);
//        actual.Should().BeSameAs(component);
//        _storageComponent.Verify(x => x.UpdateComponent(query, CancellationToken.None), Times.Once);
//        _storageComponent.Verify(x => x.GetComponent(component.Id, CancellationToken.None), Times.Once);
//        _storageCategory.Verify(x => x.GetCategory(category.Id, null, CancellationToken.None), Times.Once);
//    }

//    [Fact]
//    public async Task ThrowSpecificationsNotFoundException_WhenCreateComponent_IfMissingSpecification()
//    {
//        var categoryId = Guid.Parse("86bc4fa7-7c86-4685-9131-8436220a8dba");
//        var specificationId = Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a");
//        var specification = new List<Specification>()
//        {
//            new Specification(){Id=specificationId}
//        };
//        var command = new CreateComponentCommand(null, categoryId,  new List<CreateSpecificationValueCommand>()
//        {
//            new CreateSpecificationValueCommand(new Guid(), null)
//        });
//        var getSpecificationByCategory = _specificationService.Setup(x =>
//            x.GetSpecificationsByCategory(It.IsAny<Guid>(), CancellationToken.None));
//        getSpecificationByCategory.ReturnsAsync(specification);

//        await _sut.Invoking(x => x.CreateComponent(command, CancellationToken.None))
//            .Should().ThrowAsync<CollectionEntitiesNotFoundException>();
//    }

//    [Fact]
//    public async Task ThrowCategoryNotFoundException_WhenUpdateComponent_IfCategoryIsNull()
//    {
//        var componentId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
//        var componentCommand = new UpdateComponentCommand(componentId, "component", Guid.Empty);
//        var component = new Component();

//        var getComponentSetup = _storageComponent.Setup(x =>
//            x.GetComponent(It.IsAny<Guid>(), CancellationToken.None));
//        getComponentSetup.ReturnsAsync(component);

//        await _sut.Invoking(x => x.UpdateComponent(componentCommand, CancellationToken.None))
//            .Should().ThrowAsync<EntityNotFoundException>();
//    }

//    [Fact]
//    public async Task ThrowCategoryNotFoundException_WhenUpdateComponent_IfComponentIsNull()
//    {
//        var categoryId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
//        var componentCommand = new UpdateComponentCommand(Guid.Empty, "component", categoryId);

//        await _sut.Invoking(x => x.UpdateComponent(componentCommand, CancellationToken.None))
//            .Should().ThrowAsync<EntityNotFoundException>();
//    }
//}