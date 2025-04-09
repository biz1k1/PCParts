using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PCParts.API.Model;
using PCParts.API.Model.ResponseType;
using PCParts.Application.Model.Command;
using PCParts.Application.Services.SpecificationService;

namespace PCParts.API.Controllers
{
    [ApiController]
    [Route("SpecificationsValue")]
    public class SpecificationValueController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISpecificationService _specificationService;

        public SpecificationValueController(
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
        public async Task<IActionResult> CreateSpecificationValue(
            [FromBody] CreateSpecificationValue request,
            CancellationToken cancellationToken)
        {
            var command = new CreateSpecificationValueCommand(request.ComponentId,request.SpeicificationId, request.Value);
            var specification = await _specificationService.CreateSpecificationValue(command, cancellationToken);
            return Created("Components/{componentId}", _mapper.Map<SpecificationValue>(specification));
        }
    }
}
