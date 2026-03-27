using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletModel1A;

public record PayWithBalance1ACommand(WalletRequestModel Request) : IRequest<bool>;
