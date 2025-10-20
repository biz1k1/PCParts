namespace PCParts.API.Model.Models;

public record UpdateSpecificationValue
{
    public Guid SpecificationValueId { get; init; }
    public string Value { get; init; }
}
