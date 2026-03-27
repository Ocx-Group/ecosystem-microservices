using Asp.Versioning;
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

    // TODO: Implement when Application layer commands/queries are created

    [HttpGet("GetAllWalletsRequests")]
    public Task<IActionResult> GetAllWalletsRequests()
        => throw new NotImplementedException();

    [HttpGet("{id:int}")]
    public Task<IActionResult> GetAllWalletRequestByAffiliateId(int id)
        => throw new NotImplementedException();

    [HttpPost]
    public Task<IActionResult> CreateAsync([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("processOption")]
    public Task<IActionResult> ProcessOptionRequest([FromQuery] int option, [FromBody] List<long> ids)
        => throw new NotImplementedException();

    [HttpPost("createWalletRequestRevert")]
    public Task<IActionResult> CreateWalletRequestRevert([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPut("createWalletRequestRevert")]
    public Task<IActionResult> CreateWalletRequestRevertMobile([FromBody] object request)
        => throw new NotImplementedException();

    [HttpGet("getAllWalletRequestRevertTransaction")]
    public Task<IActionResult> GetAllWalletRequestRevertTransaction()
        => throw new NotImplementedException();

    [HttpPost("administrativePaymentAsync")]
    public Task<IActionResult> AdministrativePaymentAsync([FromBody] long[] requestIds)
        => throw new NotImplementedException();
}
