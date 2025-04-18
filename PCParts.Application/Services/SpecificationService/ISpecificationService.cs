using PCParts.Application.Command;
using PCParts.Application.Model.Models;

namespace PCParts.Application.Services.SpecificationService;

public interface ISpecificationService
{
    Task<IEnumerable<Specification>> GetSpecificationsByCategory(Guid categoryId, CancellationToken cancellationToken);
    Task<Specification> CreateSpecification(CreateSpecificationCommand command, CancellationToken cancellationToken);
    Task<Specification> UpdateSpecification(UpdateSpecificationCommand command, CancellationToken cancellationToken);
    Task RemoveSpecification(Guid id, CancellationToken cancellationToken);
}