using PCParts.Application.Model.Models;
using PCParts.Domain.Enum;

namespace PCParts.Application.DomainEvents;

public class ComponentDomainEvent : DomainEventBase<ComponentDomainEvent.ComponentPayload>
{
    public ComponentDomainEvent(ComponentPayload payload) : base(payload)
    {
    }

    public override DomainEventType EventType => DomainEventType.ComponentCreated;

    public static ComponentDomainEvent EventCreated(Component component,
        SpecificationValue specificationValue)
    {
        var payload = new ComponentPayload
        {
            ComponentId = component.Id,
            SpecificationValue = new ComponentSpecificationValue
            {
                SpecificationValueId = specificationValue.Id,
                Value = specificationValue.Value
            }
        };

        return new ComponentDomainEvent(payload);
    }

    public class ComponentPayload
    {
        public Guid ComponentId { get; set; }
        public ComponentSpecificationValue SpecificationValue { get; set; }
    }

    public class ComponentSpecificationValue
    {
        public Guid SpecificationValueId { get; set; }
        public object Value { get; set; } = null!;
    }
}