using PCParts.Domain.Enum;

namespace PCParts.Application.Abstraction.DomainEvent;

public interface IDomainEvent
{
    string Content { get; }
    DomainEventType EventType { get; }
}