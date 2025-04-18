using PCParts.Application.Abstraction;
using PCParts.Application.Command;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.QueryBuilderService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Services.SpecificationService;

public class SpecificationService : ISpecificationService
{
    private readonly ICategoryStorage _categoryStorage;
    private readonly IQueryBuilderService _queryBuilderService;
    private readonly ISpecificationStorage _specificationStorage;
    private readonly ISpecificationValueStorage _specificationValueStorage;
    private readonly IValidationService _validationService;

    public SpecificationService(
        ISpecificationStorage specificationStorage,
        ICategoryStorage categoryStorage,
        ISpecificationValueStorage specificationValueStorage,
        IValidationService validationService,
        IQueryBuilderService queryBuilderService)
    {
        _specificationStorage = specificationStorage;
        _categoryStorage = categoryStorage;
        _specificationValueStorage = specificationValueStorage;
        _validationService = validationService;
        _queryBuilderService = queryBuilderService;
    }

    public async Task<IEnumerable<Specification>> GetSpecificationsByCategory(Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await _categoryStorage.GetCategory(categoryId, cancellationToken);
        if (category is null)
        {
            throw new EntityNotFoundException(nameof(category), categoryId);
        }

        var specifications = await _specificationStorage.GetSpecificationsByCategory(categoryId, cancellationToken);
        return specifications;
    }

    public async Task<Specification> CreateSpecification(CreateSpecificationCommand command,
        CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var category = await _categoryStorage.GetCategory(command.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new EntityNotFoundException(nameof(category), command.CategoryId);
        }

        var specification = await _specificationStorage.CreateSpecification(command.CategoryId,
            command.Name, command.Type, cancellationToken);
        return specification;
    }

    public async Task RemoveSpecification(Guid id, CancellationToken cancellationToken)
    {
        var specification = await _specificationStorage.GetSpecification(id, new[] { "SpecificationValues" }, cancellationToken);
        if (specification is null)
        {
            throw new EntityNotFoundException(nameof(specification), id);
        }
        if (specification.SpecificationValues is not null)
        {
            throw new RemoveEntityWithChildrenException(nameof(specification), nameof(specification.SpecificationValues));
        }

        await _specificationStorage.RemoveSpecification(specification, cancellationToken);
    }

    public async Task<Specification> UpdateSpecification(UpdateSpecificationCommand command,
        CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var specification = await _specificationStorage.GetSpecification(command.Id, null, cancellationToken);
        if (specification is null)
        {
            throw new EntityNotFoundException(nameof(specification), command.Id);
        }

        var specificationValue = await _specificationValueStorage.GetSpecificationValue(specification.Id, null, cancellationToken);
        if (specificationValue is null)
        {
            throw new EntityNotFoundException(nameof(specificationValue), specification.Id);
        }

        if (command.Type is not null)
        {
            var validType = ValidationHelper.IsValueValid(command.Type, specificationValue?.Value?.ToString() ?? string.Empty);
            if (!validType)
            {
                throw new InvalidSpecificationTypeException(specificationValue.Value, command.Type.ToString());
            }
        }

        var query = _queryBuilderService.BuildSpecificationUpdateQuery(command);
        return await _specificationStorage.UpdateSpecification(query, cancellationToken);
    }
}