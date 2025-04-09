namespace PCParts.API.Model;

public record Specification
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}