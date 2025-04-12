using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCParts.Domain.Entities;

[Table("Category")]
public class Category
{
    [Key] 
    public Guid Id { get; set; }

    [MaxLength(50)] 
    public string Name { get; set; }

    [InverseProperty(nameof(Component.Category))]
    public ICollection<Component> Components { get; set; } = [];

    [InverseProperty(nameof(Specification.Category))]
    public ICollection<Specification> Specifications { get; set; } = [];
}