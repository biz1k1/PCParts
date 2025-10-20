using AutoMapper;
using FluentAssertions;
using Moq;
using PCParts.Storage.Mapping;
using PCParts.Application.Abstraction.Storage;
using PCParts.Application.Command;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.ComponentService;
using PCParts.Application.Services.SpecificationService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;
using PCParts.Domain.Specification.Category;
using PCParts.Domain.Specification.Component;
using PCParts.Application.Services.SpecificationValueService;
using PCParts.Application.DomainEvents;

namespace PCParts.Application.Tests.Services;

public class ComponentServiceShould
{
    private readonly Mock<ICategoryStorage> _storageCategory;
    private readonly Mock<IComponentStorage> _storageComponent;
    private readonly Mock<ISpecificationService> _specificationService;
    private readonly Mock<ISpecificationValueService> _specificationValueService;
    private readonly Mock<IDomainEventsStorage> _domainEventsStorage;
    private readonly Mock<IValidationService> _validator;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mapper _mapper;
    private readonly ComponentService _sut;
    public ComponentServiceShould()
    {
        _storageComponent = new Mock<IComponentStorage>();
        _storageCategory = new Mock<ICategoryStorage>();
        _validator = new Mock<IValidationService>();
        _specificationService = new Mock<ISpecificationService>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new StorageProfile())));
        _specificationValueService = new Mock<ISpecificationValueService>();
        _domainEventsStorage = new Mock<IDomainEventsStorage>();
        _unitOfWork = new Mock<IUnitOfWork>();


        _sut = new ComponentService(
            _storageComponent.Object,
            _validator.Object,
            _specificationService.Object,
            _unitOfWork.Object,
            _mapper);
    }

    [Fact]
    public async Task ReturnCreatedComponent()
    {
        var specificationId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
        var specificationValueId = Guid.Parse("0bf6ed61-f924-4332-8d93-14acdb1c3fa3");
        var componentId = Guid.Parse("333357ef-a6ed-40ec-ae5f-44383f00ca17");
        var categoryId = Guid.Parse("d8c4180f-cccc-4c93-8352-448079b77cd0");

        var specifications = new List<Specification>()
        {
            new (){Id=specificationId,SpecificationValues = new List<SpecificationValue>()
            {
                new (){Id=specificationId}
            }}
        };

        var command = new CreateComponentCommand("Component", categoryId, new List<CreateSpecificationValueCommand>()
        {
            new (specificationId, "text")
        });

        var createSpecificationValue = new SpecificationValue()
        {
            Id = specificationValueId,
            Value = "text"
        };

        var domainComponent = new Domain.Entities.Component
        {
            Id = componentId,
            Name = "Component",
            SpecificationValues = new List<Domain.Entities.SpecificationValue>()
            {
            }
        };
        domainComponent.SpecificationValues.Add(_mapper.Map<Domain.Entities.SpecificationValue>(createSpecificationValue));

        var scopeMock = new Mock<IUnitOfWorkTransaction>();
        scopeMock
            .Setup(s => s.GetStorage<IComponentStorage>())
            .Returns(_storageComponent.Object);

        scopeMock
            .Setup(s => s.GetStorage<ISpecificationValueService>())
            .Returns(_specificationValueService.Object);

        scopeMock
            .Setup(s => s.GetStorage<IDomainEventsStorage>())
            .Returns(_domainEventsStorage.Object);

        _unitOfWork.Setup(u => u.StartScope(It.IsAny<CancellationToken>()))
            .ReturnsAsync(scopeMock.Object);

        var returnCategorySetup = _specificationService.Setup(x =>
            x.GetSpecificationsByCategory(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        returnCategorySetup.ReturnsAsync(specifications.AsEnumerable());

        var returnComponentSetup = _storageComponent.Setup(x =>
            x.CreateComponent(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        returnComponentSetup.ReturnsAsync(domainComponent);

        var returnSpecificationValue = _specificationValueService.Setup(x =>
            x.CreateSpecificationsValues(It.IsAny<Guid>(), It.IsAny<ICollection<CreateSpecificationValueCommand>>(),
                It.IsAny<CancellationToken>()));
        returnSpecificationValue.ReturnsAsync(createSpecificationValue);

        var returnDomainEventsStorage = _domainEventsStorage.Setup(x =>
            x.AddAsync(It.IsAny<ComponentDomainEvent>(), It.IsAny<CancellationToken>()));
        returnDomainEventsStorage.Returns(() => Task.CompletedTask);

        var actual = await _sut.CreateComponent(command, CancellationToken.None);
        _storageComponent.Verify(x => x.CreateComponent("Component", categoryId, CancellationToken.None), Times.Once);
        _specificationService.Verify(x => x.GetSpecificationsByCategory(categoryId, CancellationToken.None));
    }

    [Fact]
    public async Task ReturnUpdatedComponent()
    {
        var category = new Domain.Entities.Category() { Id = Guid.Parse("86bc4fa7-7c86-4685-9131-8436220a8dba") };

        var domainComponent = new Domain.Entities.Component
        {
            Id = Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a"),
            Name = "name",
            Category = category
        };

        var applicationComponent = _mapper.Map<Component>(domainComponent);

        var command = new UpdateComponentCommand(applicationComponent.Id, applicationComponent.Name);

        var updateComponentSetup = _storageComponent.Setup(x =>
            x.UpdateComponent(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));
        updateComponentSetup.ReturnsAsync(domainComponent);

        var getCategorySetup = _storageCategory.Setup(x =>
            x.GetCategory(It.IsAny<Guid>(), It.IsAny<CategoryWithComponentSpec>(), CancellationToken.None));
        getCategorySetup.ReturnsAsync(category);

        var getComponentSetup = _storageComponent.Setup(x =>
            x.GetComponent(It.IsAny<Guid>(), It.IsAny<ComponentWithSpecificationValueWithSpecificationSpec>(), CancellationToken.None));
        getComponentSetup.ReturnsAsync(domainComponent);

        var actual = await _sut.UpdateComponent(command, CancellationToken.None);
        actual.Should().BeEquivalentTo(applicationComponent);

        _storageComponent.Verify(x => x.UpdateComponent(applicationComponent.Id, applicationComponent.Name, CancellationToken.None), Times.Once);
        _storageComponent.Verify(x => x.GetComponent(applicationComponent.Id, It.IsAny<ComponentWithSpecificationValueWithSpecificationSpec>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ThrowSpecificationsNotFoundException_WhenCreateComponent_IfMissingSpecification()
    {
        var categoryId = Guid.Parse("86bc4fa7-7c86-4685-9131-8436220a8dba");
        var specificationId = Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a");
        var specification = new List<Specification>()
        {
            new (){Id=specificationId}
        };
        var command = new CreateComponentCommand(null, categoryId, new List<CreateSpecificationValueCommand>()
        {
            new (new Guid(), null)
        });
        var getSpecificationByCategory = _specificationService.Setup(x =>
            x.GetSpecificationsByCategory(It.IsAny<Guid>(), CancellationToken.None));
        getSpecificationByCategory.ReturnsAsync(specification);

        await _sut.Invoking(x => x.CreateComponent(command, CancellationToken.None))
            .Should().ThrowAsync<CollectionEntitiesNotFoundException>();
    }

    [Fact]
    public async Task ThrowCategoryNotFoundException_WhenUpdateComponent_IfComponentIsNull()
    {
        var categoryId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
        var componentName = "component";

        var componentCommand = new UpdateComponentCommand(categoryId, componentName);

        await _sut.Invoking(x => x.UpdateComponent(componentCommand, CancellationToken.None))
            .Should().ThrowAsync<EntityNotFoundException>();
    }
}

