using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Wallet;

public record PayWithBalanceModel2Command(WalletRequest Request) : IRequest<bool>;
