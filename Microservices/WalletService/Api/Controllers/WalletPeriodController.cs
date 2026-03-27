using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.WalletPeriod;
using Ecosystem.WalletService.Application.Queries.WalletPeriod;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/WalletPeriod")]
public class WalletPeriodController : BaseController
{
    private readonly IMediator _mediator;
    public WalletPeriodController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAllWalletsPeriods")]
    public async Task<IActionResult> GetAllWalletsPeriods(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllWalletPeriodsQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWalletPeriodById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetWalletPeriodByIdQuery(id), ct);
        return result is null ? Ok(Fail("The wallet period wasn't found")) : Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateWalletPeriodAsync([FromBody] CreateWalletPeriodCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWalletPeriodAsync([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteWalletPeriodCommand(id), ct);
        return result is null ? Ok(Fail("The wallet period wasn't deleted")) : Ok(Success(result));
    }
}
