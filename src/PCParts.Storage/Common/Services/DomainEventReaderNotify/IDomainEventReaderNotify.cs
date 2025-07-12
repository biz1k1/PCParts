using PCParts.Domain.Entities;
using PCParts.Storage.Common.Extensions;

namespace PCParts.Storage.Common.Services.DomainEventReaderNotify
{
    public interface IDomainEventReaderNotify
    {
        Task StartListeningNotifyAsync(CancellationToken cancellationToken);
        Task<List<DomainEvents>> GetAllNonActivatedEventsAsync(CancellationToken cancellationToken);
        Task MarkEventsAsActivatedAsync(IEnumerable<Guid> eventIds, CancellationToken cancellationToken);
        AsyncSignal EventSignal { get; }
    }
}
