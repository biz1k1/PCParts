namespace PCParts.Domain.Exceptions;

public class SpecificationValueNotFoundException(Guid specificationId) :
    DomainException(DomainErrorCode.NotFound, $"SpecificationValue with id {specificationId} was not found!");