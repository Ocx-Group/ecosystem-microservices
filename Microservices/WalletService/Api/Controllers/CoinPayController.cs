using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/CoinPay")]
public class CoinPayController : BaseController
{
    private readonly IMediator _mediator;
    public CoinPayController(IMediator mediator) => _mediator = mediator;

    // TODO: Implement when Application layer commands/queries are created

    [HttpPost("createTransaction")]
    public Task<IActionResult> CreateTransaction([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("createChannel")]
    public Task<IActionResult> CreateChannel([FromBody] object request)
        => throw new NotImplementedException();

    [HttpGet("getNetworksByIdCurrency")]
    public Task<IActionResult> GetNetworkByIdCurrency([FromQuery] int idCurrency)
        => throw new NotImplementedException();

    [HttpPost("createAddress")]
    public Task<IActionResult> CreateAddress([FromBody] object request)
        => throw new NotImplementedException();

    [HttpGet("getTransactionById")]
    public Task<IActionResult> GetTransactionById([FromQuery] int idTransaction)
        => throw new NotImplementedException();

    [HttpPost("Webhook")]
    public Task<IActionResult> Webhook()
        => throw new NotImplementedException();

    [HttpPost("sendFunds")]
    public Task<IActionResult> SendFunds([FromBody] object[] request)
        => throw new NotImplementedException();

    [HttpGet("getTransactionByReference")]
    public Task<IActionResult> GetTransactionByReference([FromQuery] string reference)
        => throw new NotImplementedException();
}
