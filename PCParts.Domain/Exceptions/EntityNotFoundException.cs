namespace PCParts.Domain.Exceptions;

public class EntityNotFoundException(string entityName, Guid entityId) :
    DomainException(DomainErrorCode.NotFound, $"{entityName} with id {entityId} was not found!");