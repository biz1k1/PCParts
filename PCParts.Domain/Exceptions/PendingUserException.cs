namespace PCParts.Domain.Exceptions;

public class PendingUserException() :
    DomainException(DomainErrorCode.Conflict,
        "The code has been sent. Wait 2 minutes before receiving the code again.");