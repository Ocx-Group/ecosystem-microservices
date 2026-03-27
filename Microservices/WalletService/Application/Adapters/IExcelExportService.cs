using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Migrate Excel generation from ClosedXML to infrastructure layer
public interface IExcelExportService
{
    MemoryStream GenerateInvoiceReport(List<(Invoice Invoice, UserInfoResponse? User)> data);
}
