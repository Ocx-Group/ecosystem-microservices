using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.WalletModel1B;
using Ecosystem.WalletService.Application.Queries.WalletModel1B;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WalletModel1BController : BaseController
{
    private readonly IMediator _mediator;
    public WalletModel1BController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetBalanceInformationByAffiliateId/{affiliateId:int}")]
    public async Task<IActionResult> GetBalanceInformationByAffiliateId(int affiliateId)
    {
        var response = await _mediator.Send(new GetModel1BBalanceQuery(affiliateId));
        return Ok(Success(response));
    }

    [HttpPost("payWithMyBalance1B")]
    public async Task<IActionResult> PayWithMyBalance([FromBody] WalletRequestModel request)
    {
        var response = await _mediator.Send(new PayWithBalance1BCommand(request));
        return response is false ? Ok(Fail("The payment could not be processed")) : Ok(Success(response));
    }

    [HttpPost("payWithMyServiceBalance")]
    public async Task<IActionResult> PayWithMyServiceBalance([FromBody] WalletRequestModel request)
    {
        var response = await _mediator.Send(new PayWithServiceBalance1BCommand(request));
        return response is false ? Ok(Fail("The payment could not be processed")) : Ok(Success(response));
    }

    [HttpPost("CreateServiceBalanceAdmin")]
    public async Task<IActionResult> CreateServiceBalanceAdmin([FromBody] CreditTransactionAdminRequest request)
    {
        var response = await _mediator.Send(new CreateServiceBalanceAdmin1BCommand(request));
        return response is false ? Ok(Fail("The payment could not be processed")) : Ok(Success(response));
    }
}
