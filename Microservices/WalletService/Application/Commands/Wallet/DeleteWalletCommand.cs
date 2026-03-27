using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Wallet;

public record DeleteWalletCommand(int Id) : IRequest<WalletDto?>;
