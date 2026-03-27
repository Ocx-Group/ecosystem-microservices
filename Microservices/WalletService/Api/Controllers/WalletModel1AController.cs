using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WalletModel1AController : BaseController
{
    private readonly IMediator _mediator;
    public WalletModel1AController(IMediator mediator) => _mediator = mediator;

    // TODO: Implement when Application layer commands/queries are created

    [HttpGet("GetBalanceInformationByAffiliateId/{affiliateId:int}")]
    public Task<IActionResult> GetBalanceInformationByAffiliateId(int affiliateId)
        => throw new NotImplementedException();

    [HttpPost("payWithMyBalance1A")]
    public Task<IActionResult> PayWithMyBalance([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("payWithMyServiceBalance")]
    public Task<IActionResult> PayWithMyServiceBalance([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("CreateServiceBalanceAdmin")]
    public Task<IActionResult> CreateServiceBalanceAdmin([FromBody] object request)
        => throw new NotImplementedException();
}
