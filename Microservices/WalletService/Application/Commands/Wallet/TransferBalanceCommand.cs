using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Wallet;

public record TransferBalanceCommand(string Encrypted) : IRequest<ServicesResponse>;
