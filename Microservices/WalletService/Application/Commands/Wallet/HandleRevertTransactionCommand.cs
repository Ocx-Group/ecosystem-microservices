using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Wallet;

public record HandleRevertTransactionCommand(int Option, int Id) : IRequest<bool>;
