using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCParts.Domain.Entities;

[Table("Category")]
public class Category
{
    [Key] public Guid Id { get; set; }

    [MaxLength(20)] public string Name { get; set; } = null!;

    [InverseProperty(nameof(Component.Category))]
    public ICollection<Component> Components { get; set; } = [];
}