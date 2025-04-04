using PCParts.Domain.Enum;

namespace PCParts.Domain.Exceptions;

public class InvalidSpecificationTypeException(object value, SpecificationDataType type) :
    DomainException(DomainErrorCode.NotFound, $"Invalid specification type {type} for {value} !");