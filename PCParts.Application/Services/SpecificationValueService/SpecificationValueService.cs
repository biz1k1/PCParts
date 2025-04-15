using PCParts.Application.AbstractionStorage;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.QueryBuilderService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Services.SpecificationValueService;

public class SpecificationValueService : ISpecificationValueService
{
    private readonly IComponentStorage _componentStorage;
    private readonly IQueryBuilderService _queryBuilderService;
    private readonly ISpecificationValueStorage _specificationValueStorage;
    private readonly IValidationService _validationService;
    private readonly ISpecificationStorage _specificationStorage;

    public SpecificationValueService(
        IComponentStorage componentStorage,
        ISpecificationValueStorage specificationValueStorage,
        IValidationService validationService,
        IQueryBuilderService queryBuilderService,
        ISpecificationStorage specificationStorage)
    {
        _specificationValueStorage = specificationValueStorage;
        _componentStorage = componentStorage;
        _validationService = validationService;
        _queryBuilderService = queryBuilderService;
        _specificationStorage = specificationStorage;
    }

    public async Task<SpecificationValue> CreateSpecificationsValues(Guid componentId,
        ICollection<CreateSpecificationValueCommand> commands, CancellationToken cancellationToken)
    {
        await _validationService.Validate(commands);

        var component = await _componentStorage.GetComponent(componentId, cancellationToken);
        if (component is null)
        {
            throw new ComponentNotFoundException(componentId);
        }

        var specificationValue = await _specificationValueStorage.CreateSpecificationValue(component.Id, commands, cancellationToken);
        return specificationValue;
    }

    public async Task<SpecificationValue> UpdateSpecificationValue(UpdateSpecificationValueCommand command,
        CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var specificationValue = await _specificationValueStorage.GetSpecificationValue(command.Id, 
            new[] { "Specification" }, cancellationToken);
        if (specificationValue is null)
        {
            throw new SpecificationValueNotFoundException(command.Id);
        }

        var specificationType = specificationValue.Specification.Type;
        var validType = ValidationHelper.IsValueValid(specificationType, command?.Value?.ToString() ?? string.Empty);
        if (!validType)
        {
            throw new InvalidSpecificationTypeException(command.Value,specificationType.ToString());
        }

        var query = _queryBuilderService.BuildSpecificationValueUpdateQuery(command);
        var updatedSpecificationValue = await _specificationValueStorage.UpdateSpecificationValue(query, cancellationToken);
        return updatedSpecificationValue;
    }
}