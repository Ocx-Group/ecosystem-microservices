using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Application.Queries.CoinPay;
using Ecosystem.WalletService.Domain.DTOs.CoinPayDto;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/CoinPay")]
public class CoinPayController : BaseController
{
    private const string NotificationProcessFailMessage = "The notification could not be processed.";
    private readonly IMediator _mediator;
    public CoinPayController(IMediator mediator) => _mediator = mediator;

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
            IdCurrency = request.CurrencyId,
            IdNetwork = request.NetworkId,
            IdExternalIdentification = CommonExtensions.GenerateUniqueId(request.AffiliateId),
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

            var result = await _mediator.Send(new ProcessCoinPayWebhookCommand(webhookRequest));
            return !result ? Ok(Fail(NotificationProcessFailMessage)) : Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error: " + ex.Message);
        }
    }

    [HttpPost("sendFunds")]
    public async Task<IActionResult> SendFunds([FromBody] SendFundRequest[] requests)
    {
        var result = new SendFundsDto();

        foreach (var request in requests)
        {
            var command = new SendCoinPayFundsCommand
            {
                IdCurrency = request.IdCurrency,
                IdNetwork = request.IdNetwork,
                Address = request.Address,
                Amount = request.Amount,
                Details = request.Details,
                AmountPlusFee = request.AmountPlusFee
            };

            var response = await _mediator.Send(command);

            if (response is { StatusCode: 1 })
                result.SuccessfulResponses.Add(response);
            else if (response is not null)
                result.FailedResponses.Add(response);
            else
                result.FailedResponses.Add(new SendFundsResponse { StatusCode = 0, Message = "Failed to send funds" });
        }

        return Ok(Success(result));
    }

    [HttpGet("getTransactionByReference")]
    public async Task<IActionResult> GetTransactionByReference([FromQuery] string reference)
    {
        var result = await _mediator.Send(new GetCoinPayTransactionByReferenceQuery(reference));
        return result is null ? Ok(Fail("The transaction not found")) : Ok(Success(result));
    }
}
