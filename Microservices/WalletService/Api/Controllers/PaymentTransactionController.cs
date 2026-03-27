using Asp.Versioning;
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

    // TODO: Implement when Application layer commands/queries are created

    [HttpPost]
    public Task<IActionResult> CreatePaymentTransactionAsync([FromBody] object request)
        => throw new NotImplementedException();

    [HttpGet("getAllWireTransfer")]
    public Task<IActionResult> GetAllWireTransfer()
        => throw new NotImplementedException();

    [HttpPost("confirmPayment")]
    public Task<IActionResult> ConfirmPayment([FromBody] object request)
        => throw new NotImplementedException();
}
