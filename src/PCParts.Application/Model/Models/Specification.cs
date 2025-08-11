using PCParts.Application.Model.Enums;

namespace PCParts.Application.Model.Models;

public class Specification
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public SpecificationDataType Type { get; set; }
    public ICollection<SpecificationValue> SpecificationValues { get; set; } = [];
}