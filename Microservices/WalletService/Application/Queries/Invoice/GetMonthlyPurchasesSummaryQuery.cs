using Ecosystem.WalletService.Domain.DTOs.InvoiceDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Invoice;

public record GetMonthlyPurchasesSummaryQuery(int Months) : IRequest<IEnumerable<MonthlyPurchasesDto>>;
