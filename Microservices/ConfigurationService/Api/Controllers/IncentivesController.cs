using Asp.Versioning;
using Ecosystem.ConfigurationService.Application.Commands.Incentive;
using Ecosystem.ConfigurationService.Application.Queries.Incentive;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.ConfigurationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class IncentivesController : BaseController
{
    private readonly IMediator _mediator;
    public IncentivesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateIncentive([FromBody] CreateIncentiveCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllIncentives(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllIncentivesQuery(), ct);
        return Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteIncentive([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteIncentiveCommand(id), ct);
        return result is null ? BadRequest(Fail("The incentive wasn't deleted")) : Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateIncentive([FromRoute] int id, [FromBody] UpdateIncentiveCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? BadRequest(Fail("The incentive wasn't updated")) : Ok(Success(result));
    }
}
