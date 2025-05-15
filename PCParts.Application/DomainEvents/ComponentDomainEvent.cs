using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCParts.Application.Model.Enum;
using PCParts.Application.Model.Models;

namespace PCParts.Application.DomainEvents
{
    public class ComponentDomainEvent
    {
        public ComponentEventType Type { get; set; }
        public Guid ComponentId { get; set; }
        public ComponentSpecificationValue SpecificationValue { get; set; }

        private ComponentDomainEvent(){}

        public static ComponentDomainEvent ComponentCreated(Component component, 
            SpecificationValue specificationValue)=>new()
        {
            Type = ComponentEventType.ComponentCreated,
            ComponentId = component.Id,
            SpecificationValue = new ComponentSpecificationValue()
            {
                SpecificationValueId = specificationValue.Id,
                Value = specificationValue.Value
            }
        };

        public class ComponentSpecificationValue
        {
            public Guid SpecificationValueId { get; set; }
            public object Value { get; set; } = null!;
        }
    }
}
