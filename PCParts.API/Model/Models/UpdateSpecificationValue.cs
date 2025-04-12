namespace PCParts.API.Model.Models;

public record UpdateSpecificationValue
{
    public Guid Id { get; init; }
    public string Value { get; init; }
}