using PCParts.Application.AbstractionStorage;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.QueryBuilderService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Services.SpecificationValueService
{
    public class SpecificationValueService : ISpecificationValueService
    {
        private readonly IComponentStorage _componentStorage;
        private readonly ISpecificationValueStorage _specificationValueStorage;
        private readonly IValidationService _validationService;
        private readonly IQueryBuilderService _queryBuilderService;

        public SpecificationValueService(
            IComponentStorage componentStorage,
            ISpecificationValueStorage specificationValueStorage,
            IValidationService validationService,
            IQueryBuilderService queryBuilderService)
        {
            _specificationValueStorage = specificationValueStorage;
            _componentStorage = componentStorage;
            _validationService = validationService;
            _queryBuilderService = queryBuilderService;
        }
        public async Task<SpecificationValue> CreateSpecificationValue(CreateSpecificationValueCommand command, CancellationToken cancellationToken)
        {
            await _validationService.Validate(command);

            string[] includes = [];
            var component = await _componentStorage.GetComponent(command.componentId, cancellationToken);
            if (component is null)
            {
                throw new ComponentNotFoundException(command.componentId);
            }

            var specification = await _specificationValueStorage.CreateSpecificationValue(command.componentId,
                command.specificationId, command.value, cancellationToken);
            return specification;
        }

        public async Task<SpecificationValue> UpdateSpecificationValue(UpdateSpecificationValueCommand command, CancellationToken cancellationToken)
        {
            await _validationService.Validate(command);

            var specificationValue = await _specificationValueStorage.GetSpecificationValue(command.Id, new []{"Specification"} ,cancellationToken);
            if (specificationValue is null)
            {
                throw new SpecificationValueNotFoundException(specificationValue.Id);
            }

            var specificationType = specificationValue.Specification.Type;
            var validType = ValidationHelper.IsValueValid(specificationType, specificationValue.Value.ToString());
            if (!validType)
            {
                throw new InvalidSpecificationTypeException(specificationValue.Value, specificationType);
            }

            var query = _queryBuilderService.BuildSpecificationValueUpdateQuery(command);
            var updatedSpecificationValue = await _specificationValueStorage.UpdateSpecificationValue(query, cancellationToken);
            return updatedSpecificationValue;
        }
    }
}
