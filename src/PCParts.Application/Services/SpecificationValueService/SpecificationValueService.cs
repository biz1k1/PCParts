using AutoMapper;
using PCParts.Application.Abstraction.Storage;
using PCParts.Application.Command;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;
using PCParts.Domain.Specification.SpecificationValue;

namespace PCParts.Application.Services.SpecificationValueService;

public class SpecificationValueService : ISpecificationValueService
{
    private readonly IComponentStorage _componentStorage;
    private readonly IMapper _mapper;
    private readonly ISpecificationValueStorage _specificationValueStorage;
    private readonly IValidationService _validationService;

    public SpecificationValueService(
        IComponentStorage componentStorage,
        ISpecificationValueStorage specificationValueStorage,
        IValidationService validationService,
        IMapper mapper)
    {
        _specificationValueStorage = specificationValueStorage;
        _componentStorage = componentStorage;
        _validationService = validationService;
        _mapper = mapper;
    }

    public async Task<SpecificationValue> CreateSpecificationsValues(Guid componentId,
        ICollection<CreateSpecificationValueCommand> commands, CancellationToken cancellationToken)
    {
        await _validationService.Validate(commands);

        var component = await _componentStorage.GetComponent(componentId, null, cancellationToken);
        if (component is null)
        {
            throw new EntityNotFoundException(nameof(component), componentId);
        }

        var values = commands.Select(
            dto => _mapper.Map<Domain.Entities.SpecificationValue>(dto));

        var specificationValue =
            await _specificationValueStorage.CreateSpecificationValue(component.Id, values, cancellationToken);

        return _mapper.Map<SpecificationValue>(specificationValue);
    }

    public async Task<SpecificationValue> UpdateSpecificationValue(UpdateSpecificationValueCommand command,
        CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var spec = new SpecificationValueWithSpecificationSpec();
        var specificationValue = await _specificationValueStorage.GetSpecificationValue(command.Id,
            spec, cancellationToken);

        if (specificationValue is null)
        {
            throw new EntityNotFoundException(nameof(specificationValue), command.Id);
        }

        var dto = _mapper.Map<SpecificationValue>(specificationValue);
        var validType = ValidationHelper.IsValueValid(dto.Specification.Type, command?.Value ?? string.Empty);
        if (!validType)
        {
            throw new InvalidSpecificationTypeException(command.Value, dto.ToString());
        }

        var changes = new Dictionary<string, object>
        {
            [nameof(command.Value)] = command.Value
        };

        var updatedSpecificationValue = await _specificationValueStorage
            .UpdateSpecificationValue(specificationValue, changes, cancellationToken);

        return _mapper.Map<SpecificationValue>(updatedSpecificationValue);
    }
}