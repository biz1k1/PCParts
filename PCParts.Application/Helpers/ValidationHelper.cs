using PCParts.Domain.Enum;

namespace PCParts.Application.Helpers;

public static class ValidationHelper
{
    public static bool IsValueValid(SpecificationDataType type, string value)
    {
        switch (type)
        {
            case SpecificationDataType.INT:
                return int.TryParse(value, out _);
            case SpecificationDataType.STRING:
                return !string.IsNullOrEmpty(value);
            case SpecificationDataType.DOUBLE:
                return double.TryParse(value, out _);
            case SpecificationDataType.BOOL:
                return bool.TryParse(value, out _);
            default:
                return false;
        }
    }
}