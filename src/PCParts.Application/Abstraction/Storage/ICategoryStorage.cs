using PCParts.Domain.Entities;
using PCParts.Domain.Specification.Base;

namespace PCParts.Application.Abstraction.Storage;

public interface ICategoryStorage
{
    Task<IEnumerable<Category>> GetCategories(ISpecification<Category> specification,
        CancellationToken cancellationToken);

    Task<Category> GetCategory(Guid id, ISpecification<Category> specification, CancellationToken cancellationToken);
    Task<Category> CreateCategory(string name, CancellationToken cancellationToken);
    Task<Category> UpdateCategory(Guid id, string name, CancellationToken cancellationToken);
    Task RemoveCategory(Category category, CancellationToken cancellationToken);
}
