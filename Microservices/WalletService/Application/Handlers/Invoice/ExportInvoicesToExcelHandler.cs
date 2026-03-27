using ClosedXML.Excel;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.Invoice;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class ExportInvoicesToExcelHandler : IRequestHandler<ExportInvoicesToExcelQuery, MemoryStream>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ExportInvoicesToExcelHandler> _logger;

    public ExportInvoicesToExcelHandler(
        IInvoiceRepository invoiceRepository,
        IAccountServiceAdapter accountServiceAdapter,
        ITenantContext tenantContext,
        ILogger<ExportInvoicesToExcelHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<MemoryStream> Handle(ExportInvoicesToExcelQuery request, CancellationToken cancellationToken)
    {
        const int batchSize = 30;
        var brandId = _tenantContext.TenantId;
        var stream = new MemoryStream();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Compras");

        int currentRow = 1;
        ConfigureHeaders(worksheet);

        decimal totalSum = 0;
        currentRow = 2;

        await foreach (var invoiceBatch in _invoiceRepository.GetInvoicesInBatches(request.StartDate, request.EndDate, batchSize, brandId))
        {
            var userTasks = invoiceBatch.Select(invoice =>
                _accountServiceAdapter.GetUserInfo(invoice.AffiliateId, brandId)
            ).ToList();

            var users = await Task.WhenAll(userTasks);

            for (int i = 0; i < invoiceBatch.Count; i++)
            {
                var invoice = invoiceBatch[i];
                var user = users[i];

                worksheet.Cell(currentRow, 1).Value = user?.UserName ?? "N/A";
                worksheet.Cell(currentRow, 2).Value = $"{user?.Name ?? "N/A"} {user?.LastName ?? ""}";
                worksheet.Cell(currentRow, 3).Value = invoice.Id;
                worksheet.Cell(currentRow, 4).Value = invoice.Status ? "Activa" : "Pendiente o Nula";
                worksheet.Cell(currentRow, 5).Value = invoice.TotalInvoice;
                worksheet.Cell(currentRow, 6).Value = invoice.PaymentMethod;
                worksheet.Cell(currentRow, 7).Value = invoice.Bank;
                worksheet.Cell(currentRow, 8).Value = invoice.ReceiptNumber;
                worksheet.Cell(currentRow, 9).Value = invoice.DepositDate.ToString();

                totalSum += invoice.TotalInvoice ?? 0;
                currentRow++;
            }
        }

        worksheet.Cell(currentRow + 1, 1).Value = "TOTAL:";
        worksheet.Cell(currentRow + 1, 5).Value = totalSum;

        FormatWorksheet(worksheet, currentRow);

        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }

    private static void ConfigureHeaders(IXLWorksheet worksheet)
    {
        var headers = new[]
        {
            "Afiliado",
            "Nombre y Apellido",
            "No. Factura",
            "Estado factura",
            "Total pagado",
            "Método de pago",
            "Banco",
            "No. Recibo",
            "Fecha de depósito"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
        }
    }

    private static void FormatWorksheet(IXLWorksheet worksheet, int lastRow)
    {
        worksheet.Column(1).Width = 15;
        worksheet.Column(2).Width = 25;
        worksheet.Column(3).Width = 12;
        worksheet.Column(4).Width = 15;
        worksheet.Column(5).Width = 15;
        worksheet.Column(6).Width = 15;
        worksheet.Column(7).Width = 15;
        worksheet.Column(8).Width = 15;
        worksheet.Column(9).Width = 15;

        worksheet.Column(5).Style.NumberFormat.Format = "$#,##0.00";
        worksheet.Column(9).Style.NumberFormat.Format = "dd/mm/yyyy";

        var table = worksheet.Range(1, 1, lastRow, 9);
        table.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        table.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        var totalRow = worksheet.Range(lastRow + 1, 1, lastRow + 1, 9);
        totalRow.Style.Font.Bold = true;
        totalRow.Style.Fill.BackgroundColor = XLColor.LightGray;
    }
}
