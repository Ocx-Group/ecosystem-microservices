using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletModel1B;

public record PayWithServiceBalance1BCommand(WalletRequestModel Request) : IRequest<bool>;
