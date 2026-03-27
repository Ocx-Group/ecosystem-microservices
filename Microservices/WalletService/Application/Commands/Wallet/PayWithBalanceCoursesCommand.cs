using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Wallet;

public record PayWithBalanceCoursesCommand(WalletRequest Request) : IRequest<bool>;
