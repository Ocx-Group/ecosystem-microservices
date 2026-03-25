using Asp.Versioning;
using Ecosystem.AccountService.Application.Commands.Role;
using Ecosystem.AccountService.Application.Queries.Role;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RoleController : BaseController
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("get_all")]
    public async Task<IActionResult> GetRoles(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRolesQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_rol")]
    public async Task<IActionResult> GetRoleById([FromQuery] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRoleByIdQuery(id), ct);
        return result is null ? Ok(Fail("The rol wasn't found")) : Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is null ? Ok(Fail("The rol wasn't created")) : Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRole([FromRoute] int id, [FromBody] UpdateRoleCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command with { Id = id }, ct);
        return result is null ? Ok(Fail("The rol wasn't updated")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRole([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteRoleCommand(id), ct);
        return result is false ? Ok(Fail("The rol wasn't deleted")) : Ok(Success(result));
    }
}
