using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.PaymentTransaction;
using Ecosystem.WalletService.Application.Queries.PaymentTransaction;
using Ecosystem.WalletService.Domain.Requests.PaymentTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/PaymentTransaction")]
public class PaymentTransactionController : BaseController
{
    private readonly IMediator _mediator;
    public PaymentTransactionController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreatePaymentTransactionAsync([FromBody] PaymentTransactionRequest request)
    {
        var command = new CreatePaymentTransactionCommand
        {
            IdTransaction = request.IdTransaction,
            AffiliateId = request.AffiliateId,
            Amount = request.Amount,
            Products = request.Products,
            CreatedAt = request.CreatedAt
        };

        var result = await _mediator.Send(command);
        return result is null ? Ok(Fail("The transaction wasn't created")) : Ok(Success(result));
    }

    [HttpGet("getAllWireTransfer")]
    public async Task<IActionResult> GetAllWireTransfer()
    {
        var result = await _mediator.Send(new GetAllWireTransfersQuery());
        return Ok(Success(result));
    }

    [HttpPost("confirmPayment")]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentTransactionRequest request)
    {
        var command = new ConfirmPaymentCommand
        {
            Id = request.Id,
            UserName = request.UserName
        };

        var result = await _mediator.Send(command);
        return result ? Ok(Success("The transaction was confirmed")) : Ok(Fail("The transaction wasn't confirmed"));
    }
}
