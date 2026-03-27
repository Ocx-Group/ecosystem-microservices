using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletWithdrawal;

public record GetAllWalletWithdrawalsQuery : IRequest<ICollection<WalletWithDrawalDto>>;
