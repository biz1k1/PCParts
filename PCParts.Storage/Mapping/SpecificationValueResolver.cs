using System.Globalization;
using AutoMapper;
using PCParts.Application.Model.Models;
using PCParts.Domain.Enum;

namespace PCParts.Storage.Mapping;

public static class SpecificationValueResolver
{
    public static object Resolve(Specification source)
    {
        if (source.Value is null)
            return null;
        if (source.Type is 0) return source.Value;

        return source.Type switch
        {
            SpecificationDataType.INT => int.TryParse(source.Value.ToString(), out var intValue)
                ? intValue
                : "Invalid INT",
            SpecificationDataType.STRING => source.Value,
            SpecificationDataType.DOUBLE => double.TryParse(source.Value.ToString(), NumberStyles.Any,
                CultureInfo.InvariantCulture, out var doubleValue)
                ? doubleValue
                : "Invalid DOUBLE",
            SpecificationDataType.BOOL => bool.TryParse(source.Value.ToString(), out var boolValue)
                ? boolValue
                : "Invalid BOOL"
        };
    }
}

public class Resolver : IValueResolver<Domain.Entities.Specification, Specification, object>
{
    public object Resolve(Domain.Entities.Specification source, Specification destination, object destMember, ResolutionContext context)
    {
        if (source.Value is null)
            return null;
        if (source.DataType is 0) return source.Value;

        return source.DataType switch
        {
            SpecificationDataType.INT => int.TryParse(source.Value.ToString(), out var intValue)
                ? intValue
                : "Invalid INT",
            SpecificationDataType.STRING => source.Value,
            SpecificationDataType.DOUBLE => double.TryParse(source.Value.ToString(), NumberStyles.Any,
                CultureInfo.InvariantCulture, out var doubleValue)
                ? doubleValue
                : "Invalid DOUBLE",
            SpecificationDataType.BOOL => bool.TryParse(source.Value.ToString(), out var boolValue)
                ? boolValue
                : "Invalid BOOL"
        };
    }
}