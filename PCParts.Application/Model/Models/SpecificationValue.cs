namespace PCParts.Application.Model.Models;

public class SpecificationValue
{
    public Guid Id { get; set; }
    public string SpecificationName { get; set; }
    public object Value { get; init; }
    public Specification Specification { get; set; }
}