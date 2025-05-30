using System.Text.Json;
using PCParts.Application.Abstraction;
using PCParts.Application.DomainEvents;
using PCParts.Domain.Entities;
using PCParts.Domain.Enum;

namespace PCParts.Storage.Storages;

public class DomainEventsStorage : IDomainEventsStorage
{
    private readonly PgContext _pgContext;

    public DomainEventsStorage(
        PgContext pgContext)
    {
        _pgContext = pgContext;
    }

    public async Task AddAsync(ComponentDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await _pgContext.AddAsync(new DomainEvents
        {
            DomainEventId = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            Content = JsonSerializer.Serialize(domainEvent),
            Type = (DomainEventType)domainEvent.Type,
            ActivityAt = null
        }, cancellationToken);

        await _pgContext.SaveChangesAsync(cancellationToken);
    }
}