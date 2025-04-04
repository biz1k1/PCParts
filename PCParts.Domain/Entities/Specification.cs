using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCParts.Domain.Enum;

namespace PCParts.Domain.Entities;

[Table("Specification")]
public class Specification
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    public SpecificationDataType DataType { get; set; }

    public Guid ComponentId { get; set; }

    [ForeignKey(nameof(ComponentId))] public Component Component { get; set; } = null!;
}