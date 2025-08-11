using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCParts.Domain.Enums;

namespace PCParts.Domain.Entities;

[Table("Specification")]
public class Specification
{
    [Key] public Guid Id { get; set; }
    [MaxLength(50)] public string Name { get; set; }
    public SpecificationDataType DataType { get; set; }
    public Guid CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))] public Category Category { get; set; }

    [InverseProperty(nameof(SpecificationValue.Specification))]
    public ICollection<SpecificationValue> SpecificationValues { get; set; } = [];
}