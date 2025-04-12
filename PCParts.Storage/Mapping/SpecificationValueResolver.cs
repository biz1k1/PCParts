using System.Globalization;
using AutoMapper;
using PCParts.Application.Model.Models;
using PCParts.Domain.Enum;


namespace PCParts.Storage.Mapping;
public class Resolver : IValueResolver<PCParts.Domain.Entities.SpecificationValue, SpecificationValue, object>
{
    public object Resolve(PCParts.Domain.Entities.SpecificationValue source, SpecificationValue destination, object destMember, ResolutionContext context)
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
            SpecificationDataType.DOUBLE => double.TryParse(source.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleValue) ? doubleValue : "Invalid DOUBLE",
            SpecificationDataType.BOOL => bool.TryParse(source.Value, out var boolValue) ? boolValue : "Invalid BOOL",
            _ => source.Value
        };
    }
}

