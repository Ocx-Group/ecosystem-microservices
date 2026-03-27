using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletRequest;

public record ProcessWalletRequestOptionCommand(int Option, List<long> Ids) : IRequest<ServicesResponse?>;
