using AutoMapper;
using PCParts.Application.Abstraction.Storage;
using PCParts.Application.Command;
using PCParts.Application.DomainEvents;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.SpecificationService;
using PCParts.Application.Services.SpecificationValueService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;
using PCParts.Domain.Specification.Component;

namespace PCParts.Application.Services.ComponentService;

public class ComponentService : IComponentService
{
    private readonly IComponentStorage _componentStorage;
    private readonly IMapper _mapper;
    private readonly ISpecificationService _specificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;

    public ComponentService(
        IComponentStorage componentStorage,
        IValidationService validationService,
        ISpecificationService specificationService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _componentStorage = componentStorage;
        _validationService = validationService;
        _specificationService = specificationService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Component>> GetComponents(CancellationToken cancellationToken)
    {
        var spec = new ComponentWithSpecificationValueWithSpecificationSpec();
        var components = await _componentStorage.GetComponents(spec, cancellationToken);
        return _mapper.Map<IEnumerable<Component>>(components);
    }

    public async Task<Component> GetComponent(Guid componentId, CancellationToken cancellationToken)
    {
        var spec = new ComponentWithSpecificationValueWithSpecificationSpec();
        var component = await _componentStorage.GetComponent(componentId, spec, cancellationToken);
        return _mapper.Map<Component>(component);
    }

    public async Task<Component> CreateComponent(CreateComponentCommand command, CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);
        await using var scope = await _unitOfWork.StartScope(cancellationToken);

        var specifications = await _specificationService
            .GetSpecificationsByCategory(command.CategoryId, cancellationToken);

        var specificationIds = new HashSet<Guid>(specifications.Select(s => s.Id));
        var missingIds = command.SpecificationValues
            .Where(v => !specificationIds.Contains(v.Id))
            .Select(v => v.Id);

        if (missingIds.Any())
        {
            throw new CollectionEntitiesNotFoundException(nameof(missingIds), missingIds);
        }

        var createComponentStorage = scope.GetStorage<IComponentStorage>();
        var createSpecificationsValues = scope.GetStorage<ISpecificationValueService>();
        var domainEventsStorage = scope.GetStorage<IDomainEventsStorage>();

        var component =
            await createComponentStorage.CreateComponent(command.Name, command.CategoryId, cancellationToken);
        var componentDTO = _mapper.Map<Component>(component);

        var specificationValue = await createSpecificationsValues
            .CreateSpecificationsValues(component.Id, command.SpecificationValues, cancellationToken);

        await domainEventsStorage.AddAsync(ComponentDomainEvent.EventCreated(componentDTO, specificationValue),
            cancellationToken);

        await scope.Commit(cancellationToken);

        return componentDTO;
    }

    public async Task<Component> UpdateComponent(UpdateComponentCommand command, CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var component = await _componentStorage.GetComponent(command.Id, null, cancellationToken);
        if (component is null)
        {
            throw new EntityNotFoundException(nameof(component), command.Id);
        }

        var updatedComponent = await _componentStorage.UpdateComponent(command.Id, command.Name, cancellationToken);
        return _mapper.Map<Component>(updatedComponent);
    }

    public async Task RemoveComponent(Guid id, CancellationToken cancellationToken)
    {
        var component = await _componentStorage.GetComponent(id, null, cancellationToken);
        if (component is null)
        {
            throw new EntityNotFoundException(nameof(component), id);
        }

        if (component.SpecificationValues.Count > 0)
        {
            throw new RemoveEntityWithChildrenException(nameof(component), nameof(component.SpecificationValues));
        }

        await _componentStorage.RemoveComponent(component, cancellationToken);
    }
}
