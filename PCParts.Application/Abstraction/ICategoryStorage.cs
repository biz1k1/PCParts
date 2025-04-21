using PCParts.Application.Command;
using PCParts.Application.Model.Models;

namespace PCParts.Application.Abstraction;

public interface ICategoryStorage
{
    Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken);
    Task<Category> GetCategory(Guid id,string[] includes, CancellationToken cancellationToken);
    Task<Category> CreateCategory(string name, CancellationToken cancellationToken);
    Task<Category> UpdateCategory(UpdateCategoryCommand command, CancellationToken cancellationToken);
    Task RemoveCategory(Category category, CancellationToken cancellationToken);
}