using System.Text.Json;
using PCParts.Application.Abstraction.DomainEvent;
using PCParts.Domain.Enum;

namespace PCParts.Application.DomainEvents;

public abstract class DomainEventBase<TPayload> : IDomainEvent
{
    protected DomainEventBase(TPayload payload)
    {
        Payload = payload;
        Content = JsonSerializer.Serialize(payload);
    }

    public TPayload Payload { get; }
    public string Content { get; }
    public abstract DomainEventType EventType { get; }
}