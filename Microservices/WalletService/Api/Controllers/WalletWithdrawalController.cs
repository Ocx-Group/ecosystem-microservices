using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.WalletWithdrawal;
using Ecosystem.WalletService.Application.Queries.WalletWithdrawal;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/WalletWithDrawal")]
public class WalletWithdrawalController : BaseController
{
    private readonly IMediator _mediator;
    public WalletWithdrawalController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAllWalletsWithdrawals")]
    public async Task<IActionResult> GetAllWalletsWithdrawals(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllWalletWithdrawalsQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWalletWithdrawalById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetWalletWithdrawalByIdQuery(id), ct);
        return result is null ? Ok(Fail("The wallet with drawal wasn't found")) : Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateWalletWithdrawalAsync([FromBody] CreateWalletWithdrawalCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateWalletWithdrawalAsync([FromRoute] int id, [FromBody] UpdateWalletWithdrawalCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The wallet with drawal wasn't updated")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWalletWithdrawalAsync([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteWalletWithdrawalCommand(id), ct);
        return result is null ? Ok(Fail("The wallet with drawal wasn't deleted")) : Ok(Success(result));
    }
}
