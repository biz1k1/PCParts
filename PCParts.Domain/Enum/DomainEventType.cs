namespace PCParts.Domain.Enum;

public enum DomainEventType
{
    ComponentCreated = 101,
    ComponentUpdated = 102,
    ComponentDeleted = 103,

    PendingUserCreated = 201,
    PendingUserUpdated = 202,
    PendingUserDeleted = 203
}