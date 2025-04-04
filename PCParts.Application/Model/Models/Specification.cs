using PCParts.Domain.Enum;

namespace PCParts.Application.Model.Models;

public class Specification
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public object Value { get; set; }
    public SpecificationDataType Type { get; set; }
}