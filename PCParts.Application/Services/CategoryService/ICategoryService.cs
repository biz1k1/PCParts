using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;

namespace PCParts.Application.Services.CategoryService;

public interface ICategoryService
{
    Task<Category> CreateCategory(CreateCategoryCommand command, CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken);
    Task<Category?> GetCategory(Guid id, CancellationToken cancellationToken);
    Task<Category> UpdateCategory(UpdateCategoryCommand category, CancellationToken cancellationToken);
    Task RemoveCategory(Guid id, CancellationToken cancellationToken);
}