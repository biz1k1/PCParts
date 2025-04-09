using System.ComponentModel;
using PCParts.Application.AbstractionStorage;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.QueryBuilderService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Enum;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Services.SpecificationService;
public class SpecificationService : ISpecificationService
{
    private readonly ICategoryStorage _categoryStorage;
    private readonly IComponentStorage _componentStorage;
    private readonly IQueryBuilderService _queryBuilderService;
    private readonly ISpecificationStorage _specificationStorage;
    private readonly IValidationService _validationService;

    public SpecificationService(
        ISpecificationStorage specificationStorage,
        IComponentStorage componentStorage,
        ICategoryStorage categoryStorage,
        IValidationService validationService,
        IQueryBuilderService queryBuilderService)
    {
        _specificationStorage = specificationStorage;
        _componentStorage = componentStorage;
        _categoryStorage = categoryStorage;
        _validationService = validationService;
        _queryBuilderService = queryBuilderService;
    }

    public async Task<Specification> CreateSpecification(CreateSpecificationCommand command,
        CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var category = await _categoryStorage.GetCategory(command.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new CategoryNotFoundException(command.CategoryId);
        }

        var specification = await _specificationStorage.CreateSpecification(command.CategoryId,
            command.Name, command.DataType, cancellationToken);
        return specification;
    }

    public async Task<SpecificationValue> CreateSpecificationValue(CreateSpecificationValueCommand command,
        CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        string[] includes = [];
        var component = await _componentStorage.GetComponent(command.componentId, includes, cancellationToken);
        if (component is null)
        {
            throw new ComponentNotFoundException(command.componentId);
        }

        var specification = await _specificationStorage.CreateSpecificationValue(command.componentId,
            command.specificationId,command.value, cancellationToken);
        return specification;
    }

    public async Task RemoveSpecification(Guid id, CancellationToken cancellationToken)
    {
        var specification = await _specificationStorage.GetSpecification(id, cancellationToken);
        if (specification is null)
        {
            throw new SpecificationNotFoundException(id);
        }

        await _specificationStorage.RemoveSpecification(specification, cancellationToken);
    }

    public async Task<Specification> UpdateSpecification(UpdateSpecificationCommand command,
        CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var specification = await _specificationStorage.GetSpecification(command.Id, cancellationToken);
        if (specification is null)
        {
            throw new SpecificationNotFoundException(command.Id);
        }

        var query = _queryBuilderService.BuildSpecificationUpdateQuery(command);
        return await _specificationStorage.UpdateSpecification(query, cancellationToken);
    }
}