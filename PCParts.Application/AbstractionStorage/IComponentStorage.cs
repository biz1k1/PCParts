using PCParts.Application.Model.Models;
using PCParts.Application.Model.QueryModel;

namespace PCParts.Application.AbstractionStorage;

public interface IComponentStorage
{
    Task<IEnumerable<Component>> GetComponents(CancellationToken cancellationToken);
    Task<Component> GetComponent(Guid componentId, CancellationToken cancellationToken);
    Task<Component> CreateComponent(string name, Guid categoryId, CancellationToken cancellationToken);
    Task<Component> UpdateComponent(UpdateQuery query, CancellationToken cancellationToken);
    Task RemoveComponent(Component component, CancellationToken cancellationToken);
}