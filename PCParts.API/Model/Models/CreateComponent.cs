namespace PCParts.API.Model.Models;

public record CreateComponent
{
    public string Name { get; init; }
    public Guid CategoryId { get; init; }
    public ICollection<CreateSpecificationValue> SpecificationValues { get; init; } = [];
}