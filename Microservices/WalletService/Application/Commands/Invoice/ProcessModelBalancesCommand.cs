using Ecosystem.WalletService.Domain.DTOs.InvoiceDto;
using Ecosystem.WalletService.Domain.Requests.InvoiceRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Invoice;

public record ProcessModelBalancesCommand(ModelBalancesAndInvoicesRequest Request) : IRequest<ModelBalancesAndInvoicesDto?>;
