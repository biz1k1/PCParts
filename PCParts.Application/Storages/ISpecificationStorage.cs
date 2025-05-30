using PCParts.Domain.Entities;
using PCParts.Domain.Enum;
using PCParts.Domain.Specification.Base;

namespace PCParts.Application.Storages;

public interface ISpecificationStorage
{
    Task<Specification> GetSpecification(Guid id, ISpecification<Specification> specification,
        CancellationToken cancellationToken);

    Task<IEnumerable<Specification>> GetSpecificationsByCategory(Guid categoryId,
        CancellationToken cancellationToken);

    Task<Specification> CreateSpecification(Guid categoryId, string name,
        SpecificationDataType dataType, CancellationToken cancellationToken);

    Task<Specification> UpdateSpecification(Specification specification, Dictionary<string, object> changes,
        CancellationToken cancellationToken);

    Task RemoveSpecification(Specification specification, CancellationToken cancellationToken);
}