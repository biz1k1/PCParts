using PCParts.Application.Model.Enums;

namespace PCParts.API.Model.Models;

public record CreateSpecification
{
    public Guid CategoryId { get; init; }
    public string Name { get; init; }
    public SpecificationDataType Type { get; init; }
}