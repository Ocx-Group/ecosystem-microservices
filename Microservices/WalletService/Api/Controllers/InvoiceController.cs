using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.Invoice;
using Ecosystem.WalletService.Application.Queries.Invoice;
using Ecosystem.WalletService.Domain.Requests.InvoiceRequest;
using Ecosystem.WalletService.Domain.Requests.PaginationRequest;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
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

    [HttpGet("GetAllInvoicesByUserId")]
    public async Task<IActionResult> GetAllInvoicesByUserId([FromQuery] int id)
    {
        var result = await _mediator.Send(new GetAllInvoicesByUserIdQuery(id));
        return Ok(Success(result));
    }

    [HttpGet("GetAllInvoices")]
    public async Task<IActionResult> GetAllInvoices([FromQuery] PaginationRequest request)
    {
        var result = await _mediator.Send(new GetAllInvoicesQuery(request));
        return Ok(Success(result));
    }

    [HttpPost("RevertCoinPaymentTransactions")]
    public async Task<IActionResult> RevertCoinPaymentTransactions()
    {
        var result = await _mediator.Send(new RevertCoinPaymentTransactionsCommand());
        return result
            ? Ok(Success(result))
            : BadRequest(Fail("No transactions to revert"));
    }

    [HttpGet("GetAllInvoicesForTradingAcademyPurchases")]
    public async Task<IActionResult> GetAllInvoicesForTradingAcademyPurchases()
    {
        var result = await _mediator.Send(new GetTradingAcademyInvoicesQuery());
        return Ok(Success(result));
    }

    [HttpPost("SendInvitationsForUpcomingCourses")]
    public async Task<IActionResult> SendInvitationsForUpcomingCourses([FromQuery] string link, [FromQuery] string code)
    {
        var result = await _mediator.Send(new SendCourseInvitationsCommand(link, code));
        return Ok(Success(result));
    }

    [HttpGet("GetAllInvoicesForModelOneAndTwo")]
    public async Task<IActionResult> GetAllInvoicesForModelOneAndTwo()
    {
        var result = await _mediator.Send(new GetInvoicesModelOneAndTwoQuery());
        return Ok(Success(result));
    }

    [HttpPost("ProcessAndReturnBalancesForModels1A1B2")]
    public async Task<IActionResult> ProcessAndReturnBalancesForModels1A1B2([FromBody] ModelBalancesAndInvoicesRequest request)
    {
        var result = await _mediator.Send(new ProcessModelBalancesCommand(request));
        return result is not null
            ? Ok(Success(result))
            : BadRequest(Fail("Error processing model balances"));
    }

    [HttpGet("create_invoice")]
    public async Task<IActionResult> CreateInvoice([FromQuery] int invoiceId)
    {
        var pdfBytes = await _mediator.Send(new CreateInvoicePdfByIdQuery(invoiceId));
        if (pdfBytes.Length == 0)
            return NotFound(Fail("Invoice not found"));

        return File(pdfBytes, "application/pdf", $"invoice_{invoiceId}.pdf");
    }

    [HttpGet("create_invoice_by_reference")]
    public async Task<IActionResult> CreateInvoiceByReference([FromQuery] string reference)
    {
        var result = await _mediator.Send(new CreateInvoicePdfByReferenceQuery(reference));
        if (result is null || result.PdfContent is null || result.PdfContent.Length == 0)
            return NotFound(Fail("Invoice not found"));

        Response.Headers.Append("X-Brand-Id", result.BrandId.ToString());
        return File(result.PdfContent, "application/pdf", $"invoice_{reference}.pdf");
    }

    [HttpPost("HandleDebitTransaction")]
    public async Task<IActionResult> HandleDebitTransaction([FromBody] DebitTransactionRequest debitRequest)
    {
        var result = await _mediator.Send(new HandleDebitTransactionCommand(debitRequest));
        return result is not null
            ? Ok(Success(result))
            : BadRequest(Fail("Error processing debit transaction"));
    }

    [HttpGet("ExportToExcel")]
    public async Task<IActionResult> ExportInvoicesToExcel([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var stream = await _mediator.Send(new ExportInvoicesToExcelQuery(startDate, endDate));
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"invoices_{DateTime.Now:yyyyMMdd}.xlsx");
    }
}
