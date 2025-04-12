using PCParts.Application.Model.Models;
using PCParts.Application.Model.QueryModel;

namespace PCParts.Application.AbstractionStorage;

public interface ISpecificationValueStorage
{
    Task<SpecificationValue> GetSpecificationValue(Guid specificationValueId, string[] includes,
        CancellationToken cancellationToken);

    Task<IEnumerable<SpecificationValue>> GetSpecificationsValue(Guid specificationId,
        CancellationToken cancellationToken);

    Task<SpecificationValue> CreateSpecificationValue(Guid componentId, Guid specificationId,
        string value, CancellationToken cancellationToken);

    Task<SpecificationValue> UpdateSpecificationValue(UpdateQuery query, CancellationToken cancellationToken);
}