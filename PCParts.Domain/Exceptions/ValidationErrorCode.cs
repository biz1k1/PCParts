namespace PCParts.Domain.Exceptions;

public static class ValidationErrorCode
{
    public const string Empty = nameof(Empty);
    public const string TooLong = nameof(TooLong);
    public const string TooSmall= nameof(TooSmall);
    public const string Invalid = nameof(Invalid);
    public const string InvalidSpecificationType = nameof(InvalidSpecificationType);
}