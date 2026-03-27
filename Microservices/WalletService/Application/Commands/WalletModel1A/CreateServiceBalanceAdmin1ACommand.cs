using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletModel1A;

public record CreateServiceBalanceAdmin1ACommand(CreditTransactionAdminRequest Request) : IRequest<bool>;
