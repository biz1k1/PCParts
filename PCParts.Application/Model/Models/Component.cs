namespace PCParts.Application.Model.Models;

public class Component
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public Category Category { get; set; }

    public ICollection<Specification> Specifications { get; set; } = [];
}