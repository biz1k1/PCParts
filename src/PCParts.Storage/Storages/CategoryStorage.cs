using Microsoft.EntityFrameworkCore;
using PCParts.Application.Abstraction.Storage;
using PCParts.Domain.Entities;
using PCParts.Domain.Specification.Base;
using PCParts.Storage.Common.Extensions;

namespace PCParts.Storage.Storages;

public class CategoryStorage : ICategoryStorage
{
    private readonly PgContext _pgContext;

    public CategoryStorage(
        PgContext pgContext)
    {
        _pgContext = pgContext;
    }

    public async Task<Category> CreateCategory(string name,
        CancellationToken cancellationToken)
    {
        var categoryId = Guid.NewGuid();
        var category = new Category
        {
            Id = categoryId,
            Name = name
        };

        await _pgContext.Categories.AddAsync(category, cancellationToken);
        await _pgContext.SaveChangesAsync(cancellationToken);

        var newCategory = await _pgContext.Categories
            .Where(x => x.Id == categoryId)
            .FirstAsync(cancellationToken);
        return newCategory;
    }

    public async Task<IEnumerable<Category>> GetCategories(
        ISpecification<Category> specification, CancellationToken cancellationToken)
    {
        var categories = await _pgContext.Categories
            .AsNoTracking()
            .ApplySpecification(specification)
            .ToArrayAsync(cancellationToken);
        return categories;
    }

    public async Task<Category?> GetCategory(Guid id, ISpecification<Category> specification,
        CancellationToken cancellationToken)
    {
        var category = await _pgContext.Categories
            .Where(x => x.Id == id)
            .ApplySpecification(specification)
            .FirstOrDefaultAsync(cancellationToken);
        return category;
    }

    public async Task<Category> UpdateCategory(Guid id, string name, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = id,
            Name = name
        };

        _pgContext.Categories.Attach(category);
        _pgContext.Entry(category).Property(c => c.Name).IsModified = true;

        return await _pgContext.Categories
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstAsync(cancellationToken);
    }

    public async Task RemoveCategory(Category category, CancellationToken cancellationToken)
    {
        _pgContext.Categories.Remove(category);
        await _pgContext.SaveChangesAsync(cancellationToken);
    }
}