using Ecosystem.WalletService.Domain.DTOs.InvoiceDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Invoice;

public record GetTradingAcademyInvoicesQuery : IRequest<IEnumerable<InvoiceTradingAcademyDto?>>;
