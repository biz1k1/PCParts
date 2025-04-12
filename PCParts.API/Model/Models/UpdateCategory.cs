namespace PCParts.API.Model.Models;

public record UpdateCategory
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}