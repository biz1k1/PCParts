using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PCParts.API.Model.Models;
using PCParts.API.Model.ResponseType;
using PCParts.Application.Command;
using PCParts.Application.Services.SpecificationValueService;

namespace PCParts.API.Controllers;

[ApiController]
[Route("SpecificationsValues")]
public class SpecificationValueController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISpecificationValueService _specificationValueService;

    public SpecificationValueController(
        ISpecificationValueService specificationValueService,
        IMapper mapper)
    {
        _specificationValueService = specificationValueService;
        _mapper = mapper;
    }

    [HttpPatch]
    [ProducesResponseType(201, Type = typeof(SpecificationValue))]
    [ProducesResponseType(400, Type = typeof(ValidationResponseBody))]
    [ProducesResponseType(404, Type = typeof(ErrorResponseBody))]
    public async Task<IActionResult> UpdateSpecificationValue(
        [FromBody] UpdateSpecificationValue request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSpecificationValueCommand(request.SpecificationId, request.Value);
        var specification = await _specificationValueService.UpdateSpecificationValue(command, cancellationToken);
        return Ok(_mapper.Map<SpecificationValue>(specification));
    }
}
