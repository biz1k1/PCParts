using PCParts.Application.Abstraction.DomainEvent;

namespace PCParts.Application.Abstraction.Storage;

public interface IDomainEventsStorage : IStorage
{
    Task AddAsync(IDomainEvent domainEvent, CancellationToken cancellationToken);
}