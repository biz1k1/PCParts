using PCParts.Application.AbstractionStorage;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.QueryBuilderService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Services.ComponentService;
public class ComponentService : IComponentService
{
    private readonly ICategoryStorage _categoryStorage;
    private readonly IComponentStorage _componentStorage;
    private readonly IQueryBuilderService _queryBuilderService;
    private readonly IValidationService _validationService;

    public ComponentService(
        IComponentStorage componentStorage,
        ICategoryStorage categoryStorage,
        IValidationService validationService,
        IQueryBuilderService queryBuilderService)
    {
        _componentStorage = componentStorage;
        _categoryStorage = categoryStorage;
        _validationService = validationService;
        _queryBuilderService = queryBuilderService;
    }

    public async Task<IEnumerable<Component>> GetComponents(CancellationToken cancellationToken)
    {
        var components = await _componentStorage.GetComponents(cancellationToken);
        return components;
    }

    public async Task<Component> GetComponent(Guid componentId, CancellationToken cancellationToken)
    {
        string[] includes = ["Specification", "Category"];
        var component = await _componentStorage.GetComponent(componentId, includes, cancellationToken);
        return component;
    }

    public async Task<Component> CreateComponent(CreateComponentCommand command, CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var category = await _categoryStorage.GetCategory(command.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new CategoryNotFoundException(command.CategoryId);
        }

        var component = await _componentStorage.CreateComponent(command.Name, command.CategoryId, cancellationToken);
        return component;
    }

    public async Task<Component> UpdateComponent(UpdateComponentCommand command, CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var component = await _componentStorage.GetComponent(command.Id, null, cancellationToken);
        if (component is null)
        {
            throw new ComponentNotFoundException(command.Id);
        }

        Category? category = null;
        if (command.CategoryId is not null)
        {
            category = await _categoryStorage.GetCategory((Guid)command.CategoryId, cancellationToken);
            component.Category = category;
            if (category is null)
            {
                throw new CategoryNotFoundException((Guid)command.CategoryId);
            }
        }

        var query = _queryBuilderService.BuildComponentUpdateQuery(command);
        var updatedComponent = await _componentStorage.UpdateComponent(query, cancellationToken);
        return updatedComponent;
    }

    public async Task RemoveComponent(Guid id, CancellationToken cancellationToken)
    {
        var component = await _componentStorage.GetComponent(id, null, cancellationToken);
        if (component is null)
        {
            throw new ComponentNotFoundException(id);
        }

        await _componentStorage.RemoveComponent(component, cancellationToken);
    }
}