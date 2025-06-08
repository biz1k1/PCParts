using PCParts.Application.Abstraction.DomainEvent;
using PCParts.Domain.Enum;
using System.Text.Json;


namespace PCParts.Application.DomainEvents
{
    public abstract class DomainEventBase<TPayload>:IDomainEvent
    {
        public string Content { get; }
        public abstract DomainEventType EventType { get;}
        public TPayload Payload { get; }

        protected DomainEventBase(TPayload payload)
        {
            Payload = payload;
            Content = JsonSerializer.Serialize(payload);
        }

    }
}
