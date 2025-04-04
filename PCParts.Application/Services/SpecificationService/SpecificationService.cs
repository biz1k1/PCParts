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
    private readonly IComponentStorage _componentStorage;
    private readonly IQueryBuilderService _queryBuilderService;
    private readonly ISpecificationStorage _specificationStorage;
    private readonly IValidationService _validationService;

    public SpecificationService(
        ISpecificationStorage specificationStorage,
        IComponentStorage componentStorage,
        IValidationService validationService,
        IQueryBuilderService queryBuilderService)
    {
        _specificationStorage = specificationStorage;
        _componentStorage = componentStorage;
        _validationService = validationService;
        _queryBuilderService = queryBuilderService;
    }

    public async Task<Specification> CreateSpecification(CreateSpecificationCommand command,
        CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var component = await _componentStorage.GetComponent(command.ComponentId, null, cancellationToken);
        if (component is null)
        {
            throw new ComponentNotFoundException(command.ComponentId);
        }

        var specification = await _specificationStorage.CreateSpecification(command.ComponentId,
            command.Name, command.DataType, command.Value, cancellationToken);
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

        var validType = ValidationHelper.IsValueValid((SpecificationDataType)command.Type, specification.Value.ToString());
        if (validType is false)
        {
            throw new InvalidSpecificationTypeException(specification.Value, specification.Type);
        }

        var query = _queryBuilderService.BuildSpecificationUpdateQuery(command);
        return await _specificationStorage.UpdateSpecification(query, cancellationToken);
    }
}