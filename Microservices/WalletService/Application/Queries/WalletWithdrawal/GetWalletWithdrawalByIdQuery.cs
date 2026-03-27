using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletWithdrawal;

public record GetWalletWithdrawalByIdQuery(int Id) : IRequest<WalletWithDrawalDto?>;
