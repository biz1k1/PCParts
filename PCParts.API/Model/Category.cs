namespace PCParts.API.Model;

public record Category
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public ICollection<Component> Components { get; set; }
}