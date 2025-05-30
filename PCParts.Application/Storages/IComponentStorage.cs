using PCParts.Application.Abstraction;
using PCParts.Domain.Entities;
using PCParts.Domain.Specification.Base;

namespace PCParts.Application.Storages;

public interface IComponentStorage : IStorage
{
    Task<IEnumerable<Component>> GetComponents(ISpecification<Component> specification,
        CancellationToken cancellationToken);

    Task<Component> GetComponent(Guid componentId, ISpecification<Component> specification,
        CancellationToken cancellationToken);

    Task<Component> CreateComponent(string name, Guid categoryId, CancellationToken cancellationToken);
    Task<Component> UpdateComponent(Guid id, string name, CancellationToken cancellationToken);
    Task RemoveComponent(Component component, CancellationToken cancellationToken);
}