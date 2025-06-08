using PCParts.Application.Model.Models;
using PCParts.Domain.Enum;

namespace PCParts.Application.DomainEvents;

class RegistrationPendingUserEvent : DomainEventBase<RegistrationPendingUserEvent.PendingUserPayload>
{
    public override DomainEventType EventType => DomainEventType.PendingUserCreated;

    public RegistrationPendingUserEvent(PendingUserPayload payload) : base(payload)
    {
    }

    public static RegistrationPendingUserEvent EventCreated(PendingUser user)
    {
        var payload = new PendingUserPayload()
        {
            UserId = user.Id,
            Phone = user.Phone,
        };

        return new RegistrationPendingUserEvent(payload);
    }

    public class PendingUserPayload
    {
        public Guid UserId { get; set; }
        public string Phone { get; set; }
    }
}