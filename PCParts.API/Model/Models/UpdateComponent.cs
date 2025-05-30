namespace PCParts.API.Model.Models;

public record UpdateComponent
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}