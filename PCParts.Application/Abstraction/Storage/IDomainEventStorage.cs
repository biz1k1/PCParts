using PCParts.Application.Abstraction.DomainEvent;
using PCParts.Application.DomainEvents;

namespace PCParts.Application.Abstraction.Storage;

public interface IDomainEventsStorage : IStorage
{
    Task AddAsync(IDomainEvent domainEvent, CancellationToken cancellationToken);
}