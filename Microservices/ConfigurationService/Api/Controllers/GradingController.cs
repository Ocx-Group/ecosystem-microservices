using Asp.Versioning;
using Ecosystem.ConfigurationService.Application.Commands.Grading;
using Ecosystem.ConfigurationService.Application.Queries.Grading;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.ConfigurationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class GradingController : BaseController
{
    private readonly IMediator _mediator;
    public GradingController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateGrading([FromBody] CreateGradingCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGradings(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllGradingsQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_grading")]
    public async Task<IActionResult> GetGradingById([FromQuery] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetGradingByIdQuery(id), ct);
        return Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGrading([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteGradingCommand(id), ct);
        return result is null ? BadRequest(Fail("The Grading wasn't deleted")) : Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateGrading([FromRoute] int id, [FromBody] UpdateGradingCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? BadRequest(Fail("The Grading wasn't updated")) : Ok(Success(result));
    }
}
