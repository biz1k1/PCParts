namespace PCParts.API.Model.Models;

public record CreateComponent
{
    public string Name { get; init; } = null!;
    public Guid CategoryId { get; init; }
}