using PCParts.Domain.Enum;

namespace PCParts.API.Model;

public class CreateSpecification
{
    public string Name { get; init; }
    public SpecificationDataType Type { get; init; }
    public Guid CategoryId { get; init; }
}