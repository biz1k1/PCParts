namespace PCParts.Domain.Exceptions;

public class ComponentNotFoundException(Guid componentId) :
    DomainException(DomainErrorCode.NotFound, $"Component with id {componentId} was not found!");