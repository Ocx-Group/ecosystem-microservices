using Asp.Versioning;
using Ecosystem.AccountService.Application.Commands.Privilege;
using Ecosystem.AccountService.Application.Queries.Privilege;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PrivilegeController : BaseController
{
    private readonly IMediator _mediator;

    public PrivilegeController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreatePrivilege([FromBody] CreatePrivilegeCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is null ? Ok(Fail("The privilege wasn't created")) : Ok(Success(result));
    }

    [HttpGet("get_privilege_rol_id")]
    public async Task<IActionResult> GetPrivilegesByRolId([FromQuery] int rolId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPrivilegesByRolIdQuery(rolId), ct);
        return Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePrivilege([FromRoute] int id, [FromBody] UpdatePrivilegeCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command with { Id = id }, ct);
        return result is null ? Ok(Fail("The privilege wasn't updated")) : Ok(Success(result));
    }

    [HttpGet("get_all")]
    public async Task<IActionResult> GetAllPrivileges(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllPrivilegesQuery(), ct);
        return Ok(Success(result));
    }
}
