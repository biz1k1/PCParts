namespace PCParts.API.Model.Models;

public record Category
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public ICollection<Component> Components { get; init; }
}