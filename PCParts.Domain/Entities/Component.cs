using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCParts.Domain.Entities;

[Table("Component")]
public class Component
{
    [Key] public Guid Id { get; set; }

    [MaxLength(20)] public string Name { get; set; } = null!;

    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))] public Category Category { get; set; } = null!;

    [InverseProperty(nameof(Specification.Component))]
    public ICollection<Specification> Specifications { get; set; } = [];
}