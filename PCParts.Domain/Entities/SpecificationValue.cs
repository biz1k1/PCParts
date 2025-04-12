using System.ComponentModel.DataAnnotations.Schema;

namespace PCParts.Domain.Entities;

[Table("SpecificationValue")]
public class SpecificationValue
{
    public Guid Id { get; set; }
    public string Value { get; set; }
    public Guid ComponentId { get; set; }
    public Guid SpecificationId { get; set; }

    [ForeignKey(nameof(ComponentId))] public Component Component { get; set; }

    [ForeignKey(nameof(SpecificationId))]
    [InverseProperty(nameof(Specification.SpecificationValues))]
    public Specification Specification { get; set; }
}