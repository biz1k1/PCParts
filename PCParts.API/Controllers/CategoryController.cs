using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PCParts.API.Model;
using PCParts.API.Model.ResponseType;
using PCParts.Application.Model.Command;
using PCParts.Application.Services.CategoryService;

namespace PCParts.API.Controllers;

[ApiController]
[Route("Categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoryController(
        ICategoryService categoryService,
        IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Category))]
    [ProducesResponseType(404, Type = typeof(ErrorResponseBody))]
    public async Task<IActionResult> CreateCategory(
        [FromQuery] CreateCategory request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.Name);
        var category = await _categoryService.CreateCategory(command, cancellationToken);
        return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, _mapper.Map<Category>(category));
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Category))]
    public async Task<IActionResult> GetCategories(
        CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategories(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<Category>>(categories));
    }

    [HttpGet("{categoryId:guid}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    public async Task<IActionResult> GetCategory(
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var category = await _categoryService.GetCategory(categoryId, cancellationToken);
        return Ok(_mapper.Map<Category>(category));
    }

    [HttpPatch]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(400, Type = typeof(ValidationResponseBody))]
    [ProducesResponseType(404, Type = typeof(ErrorResponseBody))]
    public async Task<IActionResult> UpdateCategory(
        [FromBody] UpdateCategory request,
        CancellationToken cancellationToken)
    {
        var updateCommand = new UpdateCategoryCommand(request.Id, request.Name);
        var category = await _categoryService.UpdateCategory(updateCommand, cancellationToken);
        return Ok(_mapper.Map<Category>(category));
    }

    [HttpDelete("{categoryId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400, Type = typeof(ValidationResponseBody))]
    [ProducesResponseType(404, Type = typeof(ErrorResponseBody))]
    public async Task<IActionResult> RemoveCategory(
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        await _categoryService.RemoveCategory(categoryId, cancellationToken);
        return NoContent();
    }
}