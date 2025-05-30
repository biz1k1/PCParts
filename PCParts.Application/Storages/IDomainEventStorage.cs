using PCParts.Application.DomainEvents;

namespace PCParts.Application.Abstraction;

public interface IDomainEventsStorage : IStorage
{
    Task AddAsync(ComponentDomainEvent domainEvent, CancellationToken cancellationToken);
}