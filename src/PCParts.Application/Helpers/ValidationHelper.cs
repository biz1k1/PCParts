using PCParts.Application.Model.Enums;

namespace PCParts.Application.Helpers;

public static class ValidationHelper
{
    public static bool IsValueValid(SpecificationDataType? type, string value)
    {
        return type switch
        {
            SpecificationDataType.IntType => int.TryParse(value, out _),
            SpecificationDataType.StringType => !(int.TryParse(value, out _) || double.TryParse(value, out _) ||
                                                  bool.TryParse(value, out _)),
            SpecificationDataType.DoubleType => double.TryParse(value, out _) &&
                                                (value.Contains('.') || value.Contains(',')),
            SpecificationDataType.BoolType => bool.TryParse(value, out _),
            _ => false
        };
    }
}
