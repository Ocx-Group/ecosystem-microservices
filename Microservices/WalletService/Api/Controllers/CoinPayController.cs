using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Application.Queries.CoinPay;
using Ecosystem.WalletService.Domain.Configuration;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/CoinPay")]
public class CoinPayController : BaseController
{
    private const string NotificationProcessFailMessage = "The notification could not be processed.";

    private readonly IMediator _mediator;
    private readonly IOptions<ApplicationConfiguration> _appSettings;
    private readonly ILogger<CoinPayController> _logger;

    public CoinPayController(
        IMediator mediator,
        IOptions<ApplicationConfiguration> appSettings,
        ILogger<CoinPayController> logger)
    {
        _mediator = mediator;
        _appSettings = appSettings;
        _logger = logger;
    }

    private string? ReadSignature()
    {
        var header = _appSettings.Value.CoinPay?.SignatureHeader;

        return string.IsNullOrEmpty(header) ? null : Request.Headers[header].FirstOrDefault();
    }

    [HttpPost("createTransaction")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        var command = new CreateCoinPayTransactionCommand
        {
            AffiliateId = request.AffiliateId,
            UserName = request.UserName,
            Amount = request.Amount,
            Products = request.Products,
            NetworkId = request.NetworkId,
            CurrencyId = request.CurrencyId
        };

        var result = await _mediator.Send(command);
        return result?.Data is null ? Ok(Fail("Error")) : Ok(result);
    }

    [HttpPost("createChannel")]
    public async Task<IActionResult> CreateChannel([FromBody] CreateTransactionRequest request)
    {
        var command = new CreateCoinPayChannelCommand
        {
            AffiliateId = request.AffiliateId,
            Amount = request.Amount,
            Products = request.Products,
            IdCurrency = request.CurrencyId,
            IdNetwork = request.NetworkId,
            TagName = request.UserName
        };

        var result = await _mediator.Send(command);
        return result?.Data is null ? Ok(Fail("The channel could not be created.")) : Ok(Success(result));
    }

    [HttpGet("getNetworksByIdCurrency")]
    public async Task<IActionResult> GetNetworkByIdCurrency([FromQuery] int idCurrency)
    {
        var result = await _mediator.Send(new GetNetworksByCurrencyQuery(idCurrency));
        return result?.Data is null ? Ok(Fail("Network not found for the provided ID.")) : Ok(result);
    }

    [HttpPost("createAddress")]
    public async Task<IActionResult> CreateAddress([FromBody] CreateAddresRequest request)
    {
        var command = new CreateCoinPayAddressCommand
        {
            IdCurrency = request.IdCurrency,
            IdNetwork = request.IdNetwork,
            IdWallet = request.IdWallet
        };

        var result = await _mediator.Send(command);
        return result?.Data is null ? Ok(Fail("The address could not be created.")) : Ok(result);
    }

    [HttpGet("getTransactionById")]
    public async Task<IActionResult> GetTransactionById([FromQuery] int idTransaction)
    {
        var result = await _mediator.Send(new GetCoinPayTransactionByIdQuery(idTransaction));
        return result?.Data is null ? Ok(Fail("Transaction not found.")) : Ok(Success(result));
    }

    [HttpPost("Webhook")]
    public async Task<IActionResult> Webhook()
    {
        Request.EnableBuffering();
        string requestBody;

        using (var reader = new StreamReader(Request.Body, leaveOpen: true))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        Request.Body.Position = 0;

        try
        {
            var webhookRequest = JsonConvert.DeserializeObject<WebhookNotificationRequest>(requestBody);
            if (webhookRequest is null)
                return Ok(Fail(NotificationProcessFailMessage));

            var result = await _mediator.Send(new ProcessCoinPayWebhookCommand(webhookRequest, ReadSignature()));
            return !result ? Ok(Fail(NotificationProcessFailMessage)) : Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error while processing the CoinPay webhook");
            return StatusCode(500, NotificationProcessFailMessage);
        }
    }

    [HttpPost("sendFunds")]
    public async Task<IActionResult> SendFunds([FromBody] WithDrawalRequest[] requests)
    {
        var result = await _mediator.Send(new SendCoinPayFundsCommand(requests));
        return Ok(Success(result));
    }

    [HttpGet("getTransactionByReference")]
    public async Task<IActionResult> GetTransactionByReference([FromQuery] string reference)
    {
        var isCredited = await _mediator.Send(new GetCoinPayTransactionByReferenceQuery(reference));
        return Ok(Success(isCredited));
    }
}
