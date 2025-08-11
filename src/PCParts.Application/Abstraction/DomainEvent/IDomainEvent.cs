using PCParts.Domain.Enums;

namespace PCParts.Application.Abstraction.DomainEvent;

public interface IDomainEvent
{
    string Content { get; }
    DomainEventType EventType { get; }
}