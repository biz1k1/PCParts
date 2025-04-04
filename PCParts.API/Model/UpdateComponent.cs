namespace PCParts.API.Model;

public class UpdateComponent
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public Guid? CategoryId { get; init; }
}