using PCParts.Application.Command;
using PCParts.Application.Model.Models;
using PCParts.Application.Model.QueryModel;

namespace PCParts.Application.Abstraction;

public interface ISpecificationValueStorage
{
    Task<SpecificationValue> GetSpecificationValue(Guid specificationValueId, string[] includes,
        CancellationToken cancellationToken);
    Task<SpecificationValue> CreateSpecificationValue(Guid componentId,
        ICollection<CreateSpecificationValueCommand> command, CancellationToken cancellationToken);
    Task<SpecificationValue> UpdateSpecificationValue(UpdateQuery query, CancellationToken cancellationToken);
}