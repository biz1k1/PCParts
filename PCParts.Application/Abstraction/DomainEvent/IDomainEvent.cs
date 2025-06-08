using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCParts.Domain.Enum;

namespace PCParts.Application.Abstraction.DomainEvent
{
    public interface IDomainEvent
    {
        string Content { get; }
        DomainEventType EventType { get; }
    }
}
