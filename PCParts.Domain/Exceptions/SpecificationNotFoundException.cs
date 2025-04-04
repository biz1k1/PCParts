namespace PCParts.Domain.Exceptions;

public class SpecificationNotFoundException(Guid specificationId) :
    DomainException(DomainErrorCode.NotFound, $"Specification with id {specificationId} was not found!");