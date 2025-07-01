using System.Globalization;
using AutoMapper;
using PCParts.Domain.Entities;
using PCParts.Domain.Enum;

namespace PCParts.Storage.Mapping;

public class Resolver : IValueResolver<SpecificationValue, Application.Model.Models.SpecificationValue, object>
{
    public object Resolve(SpecificationValue source, Application.Model.Models.SpecificationValue destination,
        object destMember, ResolutionContext context)
    {
        if (source.Specification is null)
        {
            return null;
        }

        if (source.Specification.DataType is 0)
        {
            return source.Value;
        }

        return source.Specification.DataType switch
        {
            SpecificationDataType.INT => int.TryParse(source.Value, out var intValue) ? intValue : "Invalid INT",
            SpecificationDataType.STRING => source.Value,
            SpecificationDataType.DOUBLE => double.TryParse(source.Value, NumberStyles.Any,
                CultureInfo.InvariantCulture, out var doubleValue)
                ? doubleValue
                : "Invalid DOUBLE",
            SpecificationDataType.BOOL => bool.TryParse(source.Value, out var boolValue) ? boolValue : "Invalid BOOL",
            _ => source.Value
        };
    }
}