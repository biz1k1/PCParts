namespace PCParts.API.Model.Models;

public record UpdateSpecificationValue
{
    public Guid SpecificationId { get; init; }
    public string Value { get; init; }
}
