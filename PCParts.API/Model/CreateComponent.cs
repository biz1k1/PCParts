namespace PCParts.API.Model;

public record CreateComponent
{
    public string Name { get; init; } = null!;
    public Guid CategoryId { get; init; }
}