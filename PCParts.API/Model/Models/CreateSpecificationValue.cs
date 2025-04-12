namespace PCParts.API.Model.Models;

public record CreateSpecificationValue
{
    public Guid ComponentId { get; init; }
    public Guid SpeicificationId { get; init; }
    public string Value { get; init; }
}