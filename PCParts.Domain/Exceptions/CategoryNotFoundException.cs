namespace PCParts.Domain.Exceptions;

public class CategoryNotFoundException(Guid categoryId) :
    DomainException(DomainErrorCode.NotFound, $"Category with id {categoryId} was not found!");