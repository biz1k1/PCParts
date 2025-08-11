using PCParts.Application.Model.Enums;

namespace PCParts.API.Model.Models;

public record UpdateSpecification
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public SpecificationDataType? Type { get; init; }
}