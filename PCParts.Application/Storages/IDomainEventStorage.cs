using PCParts.Application.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Application.Abstraction
{
    public interface IDomainEventsStorage:IStorage
    {
        Task AddAsync(ComponentDomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
