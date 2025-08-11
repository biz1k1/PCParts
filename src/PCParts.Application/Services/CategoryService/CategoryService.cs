using AutoMapper;
using PCParts.Application.Abstraction.Storage;
using PCParts.Application.Command;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;
using PCParts.Domain.Specification.Category;

namespace PCParts.Application.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly ICategoryStorage _categoryStorage;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;

    public CategoryService(
        ICategoryStorage categoryStorage,
        IValidationService validationService,
        IMapper mapper)
    {
        _categoryStorage = categoryStorage;
        _validationService = validationService;
        _mapper = mapper;
    }

    public async Task<Category> CreateCategory(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);
        var category = await _categoryStorage.CreateCategory(command.Name, cancellationToken);
        return _mapper.Map<Category>(category);
    }

    public async Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _categoryStorage.GetCategories(null, cancellationToken);
        return _mapper.Map<IEnumerable<Category>>(categories);
    }

    public async Task<Category?> GetCategory(Guid id, CancellationToken cancellationToken)
    {
        var spec = new CategoryWithComponentSpec();
        var category = await _categoryStorage.GetCategory(id, spec, cancellationToken);
        return _mapper.Map<Category>(category);
    }

    public async Task<Category> UpdateCategory(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        await _validationService.Validate(command);

        var category = await _categoryStorage.GetCategory(command.Id, null, cancellationToken);
        if (category is null)
        {
            throw new EntityNotFoundException(nameof(category), command.Id);
        }

        var updatedCategory = await _categoryStorage.UpdateCategory(command.Id, command.Name, cancellationToken);
        return _mapper.Map<Category>(updatedCategory);
    }

    public async Task RemoveCategory(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categoryStorage.GetCategory(id, null, cancellationToken);
        if (category is null)
        {
            throw new EntityNotFoundException(nameof(category), id);
        }

        if (category.Components.Count > 0)
        {
            throw new RemoveEntityWithChildrenException(nameof(category), nameof(category.Components));
        }

        await _categoryStorage.RemoveCategory(category, cancellationToken);
    }
}
