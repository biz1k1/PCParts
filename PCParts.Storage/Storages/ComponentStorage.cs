using Microsoft.EntityFrameworkCore;
using PCParts.Application.Storages;
using PCParts.Domain.Entities;
using PCParts.Domain.Specification.Base;
using PCParts.Storage.Extensions;

namespace PCParts.Storage.Storages;

public class ComponentStorage : IComponentStorage
{
    private readonly PgContext _pgContext;

    public ComponentStorage(
        PgContext pgContext)
    {
        _pgContext = pgContext;
    }

    public async Task<Component> CreateComponent(string name, Guid categoryId, CancellationToken cancellationToken)
    {
        var componentId = Guid.NewGuid();

        var component = new Component
        {
            Id = componentId,
            Name = name,
            CategoryId = categoryId
        };

        await _pgContext.Components.AddAsync(component, cancellationToken);
        await _pgContext.SaveChangesAsync(cancellationToken);

        return await _pgContext.Components
            .Where(x => x.Id == componentId)
            .FirstAsync(cancellationToken);
    }

    public async Task<Component?> GetComponent(
        Guid componentId, ISpecification<Component> specification, CancellationToken cancellationToken)
    {
        return await _pgContext.Components
            .Where(x => x.Id == componentId)
            .ApplySpecification(specification)
            .FirstOrDefaultAsync(x => x.Id == componentId, cancellationToken);
    }

    public async Task<IEnumerable<Component>> GetComponents(
        ISpecification<Component> specification, CancellationToken cancellationToken)
    {
        var e = await _pgContext.Components
            .AsNoTracking()
            .ApplySpecification(specification)
            .ToArrayAsync(cancellationToken);
        return e;
    }

    public async Task<Component> UpdateComponent(Guid id, string name, CancellationToken cancellationToken)
    {
        var component = new Component()
        {
            Id = id,
            Name = name
        };

        _pgContext.Components.Attach(component);
        _pgContext.Entry(component).Property(c => c.Name).IsModified = true;

        return await _pgContext.Components
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstAsync(cancellationToken);
    }

    public async Task RemoveComponent(Component component, CancellationToken cancellationToken)
    {
        _pgContext.Components.Remove(component);
        await _pgContext.SaveChangesAsync(cancellationToken);
    }
}