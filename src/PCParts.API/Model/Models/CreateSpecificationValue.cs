namespace PCParts.API.Model.Models;

public record CreateSpecificationValue
{
    public Guid SpecificationId { get; init; }
    public string Value { get; init; }
}