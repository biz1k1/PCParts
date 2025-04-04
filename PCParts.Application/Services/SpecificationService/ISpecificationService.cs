using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;

namespace PCParts.Application.Services.SpecificationService;

public interface ISpecificationService
{
    Task<Specification> CreateSpecification(CreateSpecificationCommand command, CancellationToken cancellationToken);
    Task<Specification> UpdateSpecification(UpdateSpecificationCommand command, CancellationToken cancellationToken);
    Task RemoveSpecification(Guid id, CancellationToken cancellationToken);
}