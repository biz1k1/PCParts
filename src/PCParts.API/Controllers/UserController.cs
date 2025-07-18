using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PCParts.API.Model.Models;
using PCParts.API.Model.ResponseType;
using PCParts.Application.Command;
using PCParts.Application.Services.PendingUserService;

namespace PCParts.API.Controllers;

[ApiController]
[Route("Users")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPendingUserService _pendingUserService;

    public UserController(
        IMapper mapper,
        IPendingUserService pendingUserService)
    {
        _mapper = mapper;
        _pendingUserService = pendingUserService;
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(PendingUser))]
    [ProducesResponseType(400, Type = typeof(ValidationResponseBody))]
    [ProducesResponseType(404, Type = typeof(ErrorResponseBody))]
    public async Task<IActionResult> CreatePendingUser(
        [FromBody] CreatePendingUser request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePendingUserCommand(request.Email, request.Password);
        var pendingUser = await _pendingUserService.CreatePendingUser(command, cancellationToken);
        return Ok(_mapper.Map<PendingUser>(pendingUser));
    }
}