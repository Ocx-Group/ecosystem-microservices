using Asp.Versioning;
using Ecosystem.WalletService.Application.Queries.InvoiceDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/InvoiceDetail")]
public class InvoiceDetailController : BaseController
{
    private readonly IMediator _mediator;
    public InvoiceDetailController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAllInvoicesDetails")]
    public async Task<IActionResult> GetAllInvoiceDetailAsync(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllInvoiceDetailsQuery(), ct);
        return Ok(Success(result));
    }
}
