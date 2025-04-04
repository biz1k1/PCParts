namespace PCParts.Application.Model.Models;

public class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Component> Components { get; set; } = [];
}