using PCParts.Application.Model.Enum;

namespace PCParts.API.Model.Models;

public record CreateSpecification
{
    public string Name { get; init; }
    public SpecificationDataType Type { get; init; }
    public Guid CategoryId { get; init; }
}