using PCParts.Application.Model.Models;
using PCParts.Domain.Enums;

namespace PCParts.Application.DomainEvents;

internal sealed class RegistrationPendingUserEvent : DomainEventBase<RegistrationPendingUserEvent.PendingUserPayload>
{
    public RegistrationPendingUserEvent(PendingUserPayload payload) : base(payload)
    {
    }

    public override DomainEventType EventType => DomainEventType.PendingUserCreated;

    public static RegistrationPendingUserEvent EventCreated(PendingUser user)
    {
        var payload = new PendingUserPayload
        {
            PendingUserId = user.Id,
            Phone = user.Phone,
            EmailConfirmationToken = user.EmailConfirmationToken
        };

        return new RegistrationPendingUserEvent(payload);
    }

    internal sealed class PendingUserPayload
    {
        public Guid PendingUserId { get; set; }
        public string Phone { get; set; }
        public string EmailConfirmationToken { get; set; }
    }
}
