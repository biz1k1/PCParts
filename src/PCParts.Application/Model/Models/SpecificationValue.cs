namespace PCParts.Application.Model.Models;

public class SpecificationValue
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public object Value { get; set; }
    public Specification Specification { get; set; }
}
