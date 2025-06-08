namespace PCParts.Domain.Exceptions;

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, Guid entityId)
        : base(DomainErrorCode.NotFound, $"{entityName} with id {entityId} was not found!")
    {
    }
}