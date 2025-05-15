using PCParts.Application.Abstraction;
using PCParts.Application.Command;
using PCParts.Application.DomainEvents;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.QueryBuilderService;
using PCParts.Application.Services.SpecificationService;
using PCParts.Application.Services.SpecificationValueService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Services.ComponentService;

public class ComponentService : IComponentService
{
    private readonly ICategoryStorage _categoryStorage;
    private readonly IComponentStorage _componentStorage;
    private readonly IQueryBuilderService _queryBuilderService;
    private readonly IValidationService _validationService;
    private readonly ISpecificationValueService _specificationValueService;
    private readonly ISpecificationService _specificationService;
    private readonly IUnitOfWork _unitOfWork;

    public ComponentService(
        IComponentStorage componentStorage,
        ICategoryStorage categoryStorage,
        IValidationService validationService,
        IQueryBuilderService queryBuilderService,
        ISpecificationValueService specificationValueService,
        ISpecificationService specificationService,
        IUnitOfWork unitOfWork)
    {
        _componentStorage = componentStorage;
        _categoryStorage = categoryStorage;
        _validationService = validationService;
        _queryBuilderService = queryBuilderService;
        _specificationValueService = specificationValueService;
        _specificationService = specificationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Component>> GetComponents(CancellationToken cancellationToken)
    {
        var components = await _componentStorage.GetComponents(cancellationToken);
        return components;
    }

    public async Task<Component> GetComponent(Guid componentId, CancellationToken cancellationToken)
    {
        var component = await _componentStorage.GetComponent(componentId, cancellationToken);
        return component;
    }

    public async Task<Component> CreateComponent(CreateComponentCommand command, CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);
        await using var scope = await _unitOfWork.StartScope(cancellationToken);

        var specifications = await _specificationService
            .GetSpecificationsByCategory(command.CategoryId, cancellationToken);
        var missingSpecification = specifications.Select(x => x.Id)
            .Except(command.SpecificationValues.Select(x => x.SpecificationId));
        if (missingSpecification.Any())
        {
            throw new CollectionEntitiesNotFoundException(nameof(missingSpecification), missingSpecification);
        }


        var createComponentStorage = scope.GetStorage<IComponentStorage>();
        var createSpecificationsValues = scope.GetStorage<ISpecificationValueService>();
        var domainEventsStorage = scope.GetStorage<IDomainEventsStorage>();

        var component = await createComponentStorage.CreateComponent(command.Name, command.CategoryId, cancellationToken);
        var specificationValue = await createSpecificationsValues
            .CreateSpecificationsValues(component.Id, command.SpecificationValues,cancellationToken);
        await domainEventsStorage.AddAsync(ComponentDomainEvent.ComponentCreated(component, specificationValue),cancellationToken);

        await scope.Commit(cancellationToken);

        return component;
    }

    public async Task<Component> UpdateComponent(UpdateComponentCommand command, CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var component = await _componentStorage.GetComponent(command.Id, cancellationToken);
        if (component is null)
        {
            throw new EntityNotFoundException(nameof(component),command.Id);
        }

        Category? category = null;
        if (command.CategoryId is not null)
        {
            category = await _categoryStorage.GetCategory((Guid)command.CategoryId, null, cancellationToken);
            component.Category = category;
            if (category is null)
            {
                throw new EntityNotFoundException(nameof(component), command.Id);
            }
        }

        var query = _queryBuilderService.BuildComponentUpdateQuery(command);
        var updatedComponent = await _componentStorage.UpdateComponent(query, cancellationToken);
        return updatedComponent;
    }

    public async Task RemoveComponent(Guid id, CancellationToken cancellationToken)
    {
        var component = await _componentStorage.GetComponent(id, cancellationToken);
        if (component is null)
        {
            throw new EntityNotFoundException(nameof(component), id);
        }
        if (component.SpecificationValues.Any())
        {
            throw new RemoveEntityWithChildrenException(nameof(component), nameof(component.SpecificationValues));
        }

        await _componentStorage.RemoveComponent(component, cancellationToken);
    }
}