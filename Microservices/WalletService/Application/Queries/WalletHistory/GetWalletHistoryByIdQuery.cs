using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletHistory;

public record GetWalletHistoryByIdQuery(int Id) : IRequest<WalletHistoryDto?>;
