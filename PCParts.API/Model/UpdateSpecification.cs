using PCParts.Domain.Enum;

namespace PCParts.API.Model;

public class UpdateSpecification
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Value { get; init; }
    public SpecificationDataType? Type { get; init; }
}