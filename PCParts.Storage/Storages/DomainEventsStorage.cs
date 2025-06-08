using System.Text.Json;
using PCParts.Application.Abstraction.DomainEvent;
using PCParts.Application.Abstraction.Storage;
using PCParts.Domain.Entities;

namespace PCParts.Storage.Storages;

public class DomainEventsStorage : IDomainEventsStorage
{
    private readonly PgContext _pgContext;

    public DomainEventsStorage(
        PgContext pgContext)
    {
        _pgContext = pgContext;
    }

    public async Task AddAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await _pgContext.AddAsync(new DomainEvents
        {
            DomainEventId = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            Content = domainEvent.Content,
            Type = domainEvent.EventType.ToString(),
            ActivityAt = null
        }, cancellationToken);

        await _pgContext.SaveChangesAsync(cancellationToken);
    }
}