using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Invoice;

public record CreateInvoicePdfByIdQuery(int InvoiceId) : IRequest<byte[]>;
