using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/Invoice")]
public class InvoiceController : BaseController
{
    private readonly IMediator _mediator;
    public InvoiceController(IMediator mediator) => _mediator = mediator;

    // TODO: Implement when Application layer commands/queries are created

    [HttpGet("GetAllInvoicesByUserId")]
    public Task<IActionResult> GetAllInvoicesByUserId([FromQuery] int id)
        => throw new NotImplementedException();

    [HttpGet("GetAllInvoices")]
    public Task<IActionResult> GetAllInvoices([FromQuery] int page, [FromQuery] int pageSize)
        => throw new NotImplementedException();

    [HttpPost("RevertCoinPaymentTransactions")]
    public Task<IActionResult> RevertCoinPaymentTransactions()
        => throw new NotImplementedException();

    [HttpGet("GetAllInvoicesForTradingAcademyPurchases")]
    public Task<IActionResult> GetAllInvoicesForTradingAcademyPurchases()
        => throw new NotImplementedException();

    [HttpPost("SendInvitationsForUpcomingCourses")]
    public Task<IActionResult> SendInvitationsForUpcomingCourses([FromQuery] string link, [FromQuery] string code)
        => throw new NotImplementedException();

    [HttpGet("GetAllInvoicesForModelOneAndTwo")]
    public Task<IActionResult> GetAllInvoicesForModelOneAndTwo()
        => throw new NotImplementedException();

    [HttpPost("ProcessAndReturnBalancesForModels1A1B2")]
    public Task<IActionResult> ProcessAndReturnBalancesForModels1A1B2([FromBody] object request)
        => throw new NotImplementedException();

    [HttpGet("create_invoice")]
    public Task<IActionResult> CreateInvoice([FromQuery] int invoiceId)
        => throw new NotImplementedException();

    [HttpGet("create_invoice_by_reference")]
    public Task<IActionResult> CreateInvoiceByReference([FromQuery] string reference)
        => throw new NotImplementedException();

    [HttpPost("HandleDebitTransaction")]
    public Task<IActionResult> HandleDebitTransaction([FromBody] object debitRequest)
        => throw new NotImplementedException();

    [HttpGet("ExportToExcel")]
    public Task<IActionResult> ExportInvoicesToExcel([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        => throw new NotImplementedException();
}
