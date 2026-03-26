using Asp.Versioning;
using Ecosystem.ConfigurationService.Application.Commands.PaymentGroup;
using Ecosystem.ConfigurationService.Application.Queries.PaymentGroup;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.ConfigurationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class GroupsController : BaseController
{
    private readonly IMediator _mediator;
    public GroupsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreatePaymentGroupCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGroups(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllPaymentGroupsQuery(), ct);
        return Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGroup([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeletePaymentGroupCommand(id), ct);
        return result is null ? BadRequest(Fail("The group wasn't deleted")) : Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateGroup([FromRoute] int id, [FromBody] UpdatePaymentGroupCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? BadRequest(Fail("The group wasn't updated")) : Ok(Success(result));
    }
}
