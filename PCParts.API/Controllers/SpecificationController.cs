using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PCParts.API.Model;
using PCParts.API.Model.ResponseType;
using PCParts.Application.Model.Command;
using PCParts.Application.Services.SpecificationService;

namespace PCParts.API.Controllers;

[ApiController]
[Route("Specifications")]
public class SpecificationController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISpecificationService _specificationService;

    public SpecificationController(
        ISpecificationService specificationService,
        IMapper mapper)
    {
        _specificationService = specificationService;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Specification))]
    [ProducesResponseType(400, Type = typeof(ValidationResponseBody))]
    [ProducesResponseType(404, Type = typeof(ErrorResponseBody))]
    public async Task<IActionResult> CreateSpecification(
        [FromBody] CreateSpecification request,
        CancellationToken cancellationToken)
    {
        var command = new CreateSpecificationCommand(request.CategoryId, request.Name, request.Type);
        var specification = await _specificationService.CreateSpecification(command, cancellationToken);
        return Created("Components/{componentId}", _mapper.Map<Specification>(specification));
    }
    
    [HttpPatch]
    [ProducesResponseType(200, Type = typeof(Specification))]
    [ProducesResponseType(400, Type = typeof(ValidationResponseBody))]
    [ProducesResponseType(404, Type = typeof(ErrorResponseBody))]
    public async Task<IActionResult> UpdateSpecification(
        [FromBody] UpdateSpecification request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSpecificationCommand(request.Id, request.Name, request.Value, request.Type);
        var specification = await _specificationService.UpdateSpecification(command, cancellationToken);
        return Ok(_mapper.Map<Specification>(specification));
    }
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = typeof(ErrorResponseBody))]
    [HttpDelete("{specificationId:guid}")]
    public async Task<IActionResult> RemoveSpecification(
        Guid specificationId,
        CancellationToken cancellationToken)
    {
        await _specificationService.RemoveSpecification(specificationId, cancellationToken);
        return NoContent();
    }
}