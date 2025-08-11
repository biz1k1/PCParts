using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCParts.Domain.Entities;

[Table("Component")]
public class Component
{
    [Key] public Guid Id { get; set; }

    [MaxLength(50)] public string Name { get; set; }

    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))] public Category Category { get; set; }

    [InverseProperty(nameof(SpecificationValue.Component))]
    public ICollection<SpecificationValue> SpecificationValues { get; set; } = [];
}
