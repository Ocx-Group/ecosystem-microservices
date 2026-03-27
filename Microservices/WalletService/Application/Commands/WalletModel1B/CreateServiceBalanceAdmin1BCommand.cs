using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletModel1B;

public record CreateServiceBalanceAdmin1BCommand(CreditTransactionAdminRequest Request) : IRequest<bool>;
