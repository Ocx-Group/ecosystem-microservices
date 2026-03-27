using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Wallet;

public record PayWithBalanceCommand(WalletRequest Request) : IRequest<bool>;
