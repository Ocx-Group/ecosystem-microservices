using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.WalletWait;
using Ecosystem.WalletService.Application.Queries.WalletWait;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/WalletWait")]
public class WalletWaitController : BaseController
{
    private readonly IMediator _mediator;
    public WalletWaitController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAllWalletsWaits")]
    public async Task<IActionResult> GetAllWalletsWaits(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllWalletWaitsQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWalletById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetWalletWaitByIdQuery(id), ct);
        return result is null ? Ok(Fail("The wallet wait wasn't found")) : Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateWalletWaitAsync([FromBody] CreateWalletWaitCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateWalletWaitAsync([FromRoute] int id, [FromBody] UpdateWalletWaitCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The wallet wait wasn't updated")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWalletWaitAsync([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteWalletWaitCommand(id), ct);
        return result is null ? Ok(Fail("The wallet wait wasn't deleted")) : Ok(Success(result));
    }
}
