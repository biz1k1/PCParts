namespace PCParts.API.Model.Models;

public record Component
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public ICollection<SpecificationValue> SpecificationValues { get; set; } = [];
}