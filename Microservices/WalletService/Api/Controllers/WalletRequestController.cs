using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.WalletRequest;
using Ecosystem.WalletService.Application.Queries.WalletRequest;
using Ecosystem.WalletService.Domain.Requests.WalletRequestRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/WalletRequest")]
public class WalletRequestController : BaseController
{
    private readonly IMediator _mediator;
    public WalletRequestController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAllWalletsRequests")]
    public async Task<IActionResult> GetAllWalletsRequests()
    {
        var result = await _mediator.Send(new GetAllWalletRequestsQuery());
        return Ok(Success(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAllWalletRequestByAffiliateId(int id)
    {
        var result = await _mediator.Send(new GetWalletRequestByAffiliateIdQuery(id));
        return Ok(Success(result!));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] WalletRequestRequest request)
    {
        var result = await _mediator.Send(new CreateWalletRequestCommand(request));
        return result.IsSuccess
            ? Ok(Success(result.Value!))
            : BadRequest(Fail(result.Error!));
    }

    [HttpPost("processOption")]
    public async Task<IActionResult> ProcessOptionRequest([FromQuery] int option, [FromBody] List<long> ids)
    {
        var result = await _mediator.Send(new ProcessWalletRequestOptionCommand(option, ids));
        return result is { Success: true }
            ? Ok(Success(result))
            : BadRequest(Fail(result?.Message ?? "Error processing option"));
    }

    [HttpPost("createWalletRequestRevert")]
    public async Task<IActionResult> CreateWalletRequestRevert([FromBody] WalletRequestRevertTransaction request)
    {
        var result = await _mediator.Send(new CreateWalletRequestRevertCommand(request));
        return result is not null
            ? Ok(Success(result))
            : BadRequest(Fail("Error creating wallet request revert"));
    }

    [HttpPut("createWalletRequestRevert")]
    public async Task<IActionResult> CreateWalletRequestRevertMobile([FromBody] WalletRequestRevertTransaction request)
    {
        var result = await _mediator.Send(new CreateWalletRequestRevertCommand(request));
        return result is not null
            ? Ok(Success(result))
            : BadRequest(Fail("Error creating wallet request revert"));
    }

    [HttpGet("getAllWalletRequestRevertTransaction")]
    public async Task<IActionResult> GetAllWalletRequestRevertTransaction()
    {
        var result = await _mediator.Send(new GetAllRevertTransactionsQuery());
        return Ok(Success(result!));
    }

    [HttpPost("administrativePaymentAsync")]
    public async Task<IActionResult> AdministrativePaymentAsync([FromBody] long[] requestIds)
    {
        var result = await _mediator.Send(new AdministrativePaymentCommand(requestIds));
        return result.IsSuccess
            ? Ok(Success(result.Value))
            : BadRequest(Fail(result.Error!));
    }
}
