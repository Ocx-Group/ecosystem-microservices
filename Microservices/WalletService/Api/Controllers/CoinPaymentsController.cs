using System.Text;
using Asp.Versioning;
using Ecosystem.WalletService.Api.Middlewares;
using Ecosystem.WalletService.Application.Commands.CoinPayments;
using Ecosystem.WalletService.Application.Queries.CoinPayments;
using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
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

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile([FromQuery] string pbntag)
    {
        var response = await _mediator.Send(new GetCoinPaymentsProfileQuery(pbntag));
        if (string.IsNullOrEmpty(response?.Error) || response.Error.Equals("ok", StringComparison.OrdinalIgnoreCase))
            return Ok(response?.Result);

        return Ok(Fail("Profile not found"));
    }

    [HttpGet("getDepositAddress")]
    public async Task<IActionResult> GetDepositAddress([FromQuery] string currency)
    {
        var response = await _mediator.Send(new GetDepositAddressQuery(currency));
        if (string.IsNullOrEmpty(response?.Error) || response.Error.Equals("ok", StringComparison.OrdinalIgnoreCase))
            return Ok(response?.Result);

        return Ok(Fail("Address not found"));
    }

    [HttpGet("getCoinBalances")]
    public async Task<IActionResult> GetCoinBalances([FromQuery] bool includeZeroBalances)
    {
        var response = await _mediator.Send(new GetCoinBalancesQuery(includeZeroBalances));
        return response?.Result is null
            ? Ok(Fail("Balances not found"))
            : Ok(Success(response.Result));
    }

    [HttpPost("createPayment")]
    public async Task<IActionResult> CreatePayment([FromBody] ConPaymentRequest request)
    {
        var command = new CreateCoinPaymentCommand
        {
            Amount = request.Amount,
            Currency1 = request.Currency1,
            Currency2 = request.Currency2,
            BuyerEmail = request.BuyerEmail,
            Address = request.Address,
            BuyerName = request.BuyerName,
            ItemName = request.ItemName,
            ItemNumber = request.ItemNumber,
            Invoice = request.Invoice,
            Custom = request.Custom,
            IpnUrl = request.IpnUrl,
            SuccessUrl = request.SuccessUrl,
            CancelUrl = request.CancelUrl,
            Products = request.Products
        };

        var response = await _mediator.Send(command);
        if (string.IsNullOrEmpty(response?.Error) || response.Error.Equals("ok", StringComparison.OrdinalIgnoreCase))
            return Ok(response?.Result);

        return Ok(Fail("Transaction could not be processed"));
    }

    [HttpGet("getTransactionInfo")]
    public async Task<IActionResult> GetTransactionInfo([FromQuery] string idTransaction, [FromQuery] bool fullInfo)
    {
        var response = await _mediator.Send(new GetCoinPaymentTransactionInfoQuery(idTransaction, fullInfo));
        return response?.Result is null
            ? Ok(Fail("Transaction not found"))
            : Ok(Success(response.Result));
    }

    [HttpPost("coinPaymentsIPN")]
    public async Task<IActionResult> CoinPaymentsIpn([FromForm] IpnRequest ipnRequest)
    {
        var headers = Request.Headers.ToDictionary(
            header => header.Key,
            header => header.Value.ToString(),
            StringComparer.OrdinalIgnoreCase);

        var rawBody = await ReadRawBody();

        var response = await _mediator.Send(new ProcessCoinPaymentsIpnCommand(ipnRequest, headers, rawBody));
        return response ? Ok("IPN OK") : BadRequest("IPN ERROR");
    }

    [HttpPost("createMassWithdrawal")]
    public async Task<IActionResult> CreateMassWithdrawal([FromBody] CoinPaymentsWithdrawalRequest[] requests)
    {
        var response = await _mediator.Send(new CreateMassWithdrawalCommand(requests));
        return response is null ? Ok(Fail("The withdrawal could not be created correctly")) : Ok(response);
    }

    /// <summary>
    /// Re-reads the body that form binding already consumed. The provider signs the bytes exactly
    /// as sent, so the digest cannot be checked against a re-serialised form collection.
    /// <see cref="IpnBufferingMiddleware"/> is what makes the stream rewindable here.
    /// </summary>
    private async Task<string> ReadRawBody()
    {
        if (!Request.Body.CanSeek)
            return string.Empty;

        Request.Body.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
        return await reader.ReadToEndAsync();
    }
}
