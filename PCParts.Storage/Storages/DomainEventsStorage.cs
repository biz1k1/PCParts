using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PCParts.Application.Abstraction;
using PCParts.Application.DomainEvents;
using PCParts.Domain.Entities;

namespace PCParts.Storage.Storages
{
    public class DomainEventsStorage:IDomainEventsStorage
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
                Type = (Domain.Enum.DomainEventType)domainEvent.Type,
                ActivityAt = null,
            },cancellationToken);

            await _pgContext.SaveChangesAsync(cancellationToken);
        }
    }
}
