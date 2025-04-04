﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PCParts.API.Model;
using PCParts.API.Model.ResponseType;
using PCParts.Application.Model.Command;
using PCParts.Application.Services.ComponentService;

namespace PCParts.API.Controllers;

[ApiController]
[Route("Components")]
public class ComponentController : ControllerBase
{
    private readonly IComponentService _componentService;
    private readonly IMapper _mapper;

    public ComponentController(
        IComponentService componentService,
        IMapper mapper)
    {
        _componentService = componentService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Component))]
    public async Task<IActionResult> GetComponents(
        CancellationToken cancellationToken)
    {
        var components = await _componentService.GetComponents(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<Component>>(components));
    }

    [HttpGet("{componentId:guid}")]
    [ProducesResponseType(200, Type = typeof(Component))]
    public async Task<IActionResult> GetComponent(
        Guid componentId,
        CancellationToken cancellationToken)
    {
        var component = await _componentService.GetComponent(componentId, cancellationToken);
        return Ok(_mapper.Map<Component>(component));
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Component))]
    [ProducesResponseType(400, Type = typeof(ValidationResponseBody))]
    [ProducesResponseType(404, Type = typeof(ErrorResponseBody))]
    public async Task<IActionResult> CreateComponent(
        [FromBody] CreateComponent request,
        CancellationToken cancellationToken)
    {
        var command = new CreateComponentCommand(request.Name, request.CategoryId);
        var component = await _componentService.CreateComponent(command, cancellationToken);
        return CreatedAtAction(nameof(GetComponent), new { categoryId = component.Id }, _mapper.Map<Component>(component));
    }

    [HttpPatch]
    [ProducesResponseType(200, Type = typeof(Component))]
    [ProducesResponseType(400, Type = typeof(ValidationResponseBody))]
    [ProducesResponseType(404, Type = typeof(ErrorResponseBody))]
    public async Task<IActionResult> UpdateComponent(
        [FromBody] UpdateComponent request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateComponentCommand(request.Id, request.Name, request.CategoryId);
        var component = await _componentService.UpdateComponent(command, cancellationToken);
        return Ok(_mapper.Map<Component>(component));
    }

    [HttpDelete("{componentId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = typeof(ErrorResponseBody))]
    public async Task<IActionResult> RemoveComponent(
        Guid componentId,
        CancellationToken cancellationToken)
    {
        await _componentService.RemoveComponent(componentId, cancellationToken);
        return NoContent();
    }
}