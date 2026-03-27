using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.WalletRetentionConfig;
using Ecosystem.WalletService.Application.Queries.WalletRetentionConfig;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/WalletRetentionConfig")]
public class WalletRetentionConfigController : BaseController
{
    private readonly IMediator _mediator;
    public WalletRetentionConfigController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAllWalletsRetentionConfig")]
    public async Task<IActionResult> GetAllWalletsRetentionConfig(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllWalletRetentionConfigsQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWalletRetentionConfigById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetWalletRetentionConfigByIdQuery(id), ct);
        return result is null ? Ok(Fail("The wallet retention wasn't found")) : Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateWalletRetentionConfigAsync([FromBody] CreateWalletRetentionConfigCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWalletRetentionConfigAsync([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteWalletRetentionConfigCommand(id), ct);
        return result is null ? Ok(Fail("The wallet retention wasn't deleted")) : Ok(Success(result));
    }
}
