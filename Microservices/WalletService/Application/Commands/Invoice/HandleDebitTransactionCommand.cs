using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Invoice;

public record HandleDebitTransactionCommand(DebitTransactionRequest Request) : IRequest<InvoicesSpResponse?>;
