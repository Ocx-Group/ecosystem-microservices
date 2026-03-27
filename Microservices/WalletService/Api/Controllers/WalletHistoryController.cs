using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.WalletHistory;
using Ecosystem.WalletService.Application.Queries.WalletHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/WalletHistory")]
public class WalletHistoryController : BaseController
{
    private readonly IMediator _mediator;
    public WalletHistoryController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAllWalletsHistories")]
    public async Task<IActionResult> GetAllWalletsHistoriesAsync(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllWalletHistoriesQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWalletHistoriesByIdAsync(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetWalletHistoryByIdQuery(id), ct);
        return result is null ? Ok(Fail("The wallet history wasn't found")) : Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateWalletHistoriesAsync([FromBody] CreateWalletHistoryCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateWalletHistoriesAsync([FromRoute] int id, [FromBody] UpdateWalletHistoryCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The wallet history wasn't updated")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWalletHistoriesAsync([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteWalletHistoryCommand(id), ct);
        return result is null ? Ok(Fail("The wallet history wasn't deleted")) : Ok(Success(result));
    }
}
