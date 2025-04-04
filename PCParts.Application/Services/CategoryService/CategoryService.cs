using PCParts.Application.AbstractionStorage;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Services.CategoryService;
public class CategoryService : ICategoryService
{
    private readonly ICategoryStorage _categoryStorage;
    private readonly IValidationService _validationService;

    public CategoryService(
        ICategoryStorage categoryStorage,
        IValidationService validationService)
    {
        _categoryStorage = categoryStorage;
        _validationService = validationService;
    }

    public async Task<Category> CreateCategory(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);
        var category = await _categoryStorage.CreateCategory(command.Name, cancellationToken);
        return category;
    }

    public async Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _categoryStorage.GetCategories(cancellationToken);
        return categories;
    }

    public async Task<Category?> GetCategory(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categoryStorage.GetCategory(id, cancellationToken);
        return category;
    }

    public async Task<Category> UpdateCategory(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var category = await _categoryStorage.GetCategory(command.Id, cancellationToken);
        if (category is null)
        {
            throw new CategoryNotFoundException(command.Id);
        }

        return await _categoryStorage.UpdateCategory(command, cancellationToken);
    }

    public async Task RemoveCategory(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categoryStorage.GetCategory(id, cancellationToken);
        if (category is null)
        {
            throw new CategoryNotFoundException(id);
        }

        await _categoryStorage.RemoveCategory(category, cancellationToken);
    }
}