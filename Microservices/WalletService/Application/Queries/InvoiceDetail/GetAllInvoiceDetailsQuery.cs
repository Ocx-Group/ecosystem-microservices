using Ecosystem.WalletService.Domain.DTOs.InvoiceDetailDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.InvoiceDetail;

public record GetAllInvoiceDetailsQuery : IRequest<ICollection<InvoiceDetailDto>>;
