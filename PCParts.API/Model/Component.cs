namespace PCParts.API.Model;

public record Component
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public Category Category { get; init; }

    public ICollection<Specification> Specifications { get; init; } = [];
}