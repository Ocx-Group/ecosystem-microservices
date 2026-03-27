using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Invoice;

public record ExportInvoicesToExcelQuery(DateTime? StartDate, DateTime? EndDate) : IRequest<MemoryStream>;
