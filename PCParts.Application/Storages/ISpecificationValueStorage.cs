using PCParts.Domain.Entities;
using PCParts.Domain.Specification.Base;

namespace PCParts.Application.Storages;

public interface ISpecificationValueStorage
{
    Task<SpecificationValue> GetSpecificationValue(Guid specificationValueId, ISpecification<SpecificationValue> spec,
        CancellationToken cancellationToken);

    Task<SpecificationValue> CreateSpecificationValue(Guid componentId,
        IEnumerable<SpecificationValue> specificationValues, CancellationToken cancellationToken);

    Task<SpecificationValue> UpdateSpecificationValue(SpecificationValue specificationValue,
        Dictionary<string, object> changes, CancellationToken cancellationToken);
}