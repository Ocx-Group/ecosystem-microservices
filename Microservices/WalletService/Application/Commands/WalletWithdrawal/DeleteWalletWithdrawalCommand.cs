using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletWithdrawal;

public record DeleteWalletWithdrawalCommand(int Id) : IRequest<WalletWithDrawalDto?>;
