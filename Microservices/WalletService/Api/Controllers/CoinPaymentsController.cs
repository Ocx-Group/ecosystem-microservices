using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/ConPayments")]
public class CoinPaymentsController : BaseController
{
    private readonly IMediator _mediator;
    public CoinPaymentsController(IMediator mediator) => _mediator = mediator;

    // TODO: Implement when Application layer commands/queries are created

    [HttpGet("profile")]
    public Task<IActionResult> GetProfile([FromQuery] string pbntag)
        => throw new NotImplementedException();

    [HttpGet("getDepositAddress")]
    public Task<IActionResult> GetDepositAddress([FromQuery] string currency)
        => throw new NotImplementedException();

    [HttpGet("getCoinBalances")]
    public Task<IActionResult> GetCoinBalances([FromQuery] bool includeZeroBalances)
        => throw new NotImplementedException();

    [HttpPost("createPayment")]
    public Task<IActionResult> CreatePayment([FromBody] object request)
        => throw new NotImplementedException();

    [HttpGet("getTransactionInfo")]
    public Task<IActionResult> GetTransactionInfo([FromQuery] string idTransaction, [FromQuery] bool fullInfo)
        => throw new NotImplementedException();

    [HttpPost("coinPaymentsIPN")]
    public Task<IActionResult> CoinPaymentsIpn()
        => throw new NotImplementedException();

    [HttpPost("createMassWithdrawal")]
    public Task<IActionResult> CreateMassWithdrawal([FromBody] object[] requests)
        => throw new NotImplementedException();
}
