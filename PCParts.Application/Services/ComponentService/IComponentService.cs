using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;

namespace PCParts.Application.Services.ComponentService;

public interface IComponentService
{
    Task<Component> CreateComponent(CreateComponentCommand command, CancellationToken cancellationToken);
    Task<IEnumerable<Component>> GetComponents(CancellationToken cancellationToken);
    Task<Component> GetComponent(Guid componentId, CancellationToken cancellationToken);
    Task<Component> UpdateComponent(UpdateComponentCommand command, CancellationToken cancellationToken);
    Task RemoveComponent(Guid id, CancellationToken cancellationToken);
}