namespace PCParts.Domain.Enum;

public enum DomainEventType
{
    ComponentCreated = 100,
    ComponentUpdated = 101,
    ComponentDeleted = 102,

    SpecificationValueCreated = 200,
    SpecificationValueUpdated = 201,
    SpecificationValueDeleted = 202
}