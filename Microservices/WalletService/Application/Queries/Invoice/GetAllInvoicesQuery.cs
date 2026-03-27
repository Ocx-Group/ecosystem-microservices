using Ecosystem.WalletService.Domain.DTOs.InvoiceDto;
using Ecosystem.WalletService.Domain.DTOs.PaginationDto;
using Ecosystem.WalletService.Domain.Requests.PaginationRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Invoice;

public record GetAllInvoicesQuery(PaginationRequest Request) : IRequest<PaginationDto<InvoiceDto>>;
