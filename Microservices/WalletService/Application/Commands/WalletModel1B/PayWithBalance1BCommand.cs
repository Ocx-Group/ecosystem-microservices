using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletModel1B;

public record PayWithBalance1BCommand(WalletRequestModel Request) : IRequest<bool>;
