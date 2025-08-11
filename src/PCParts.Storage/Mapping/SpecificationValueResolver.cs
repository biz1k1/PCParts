using System.Globalization;
using AutoMapper;
using PCParts.Domain.Entities;
using PCParts.Domain.Enums;

namespace PCParts.Storage.Mapping;

public class Resolver : IValueResolver<SpecificationValue, Application.Model.Models.SpecificationValue, object>
{
    public object Resolve(SpecificationValue source, Application.Model.Models.SpecificationValue destination,
        object destMember, ResolutionContext context)
    {
        return source.Specification is null
            ? null
            : source.Specification.DataType switch
            {
                0 => source.Value,
                SpecificationDataType.IntType => int.TryParse(source.Value, out var intValue) ? intValue : "Invalid INT",
                SpecificationDataType.StringType => source.Value,
                SpecificationDataType.DoubleType => double.TryParse(source.Value, NumberStyles.Any,
                    CultureInfo.InvariantCulture, out var doubleValue) ? doubleValue : "Invalid DOUBLE",
                SpecificationDataType.BoolType => bool.TryParse(source.Value, out var boolValue) ? boolValue : "Invalid BOOL",
                _ => source.Value
            };
    }
}
