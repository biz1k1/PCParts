using PCParts.Application.Model.Enum;

namespace PCParts.API.Model.Models;

public class UpdateSpecification
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public SpecificationDataType? Type { get; init; }
}