using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletHistory;

public record GetAllWalletHistoriesQuery : IRequest<ICollection<WalletHistoryDto>>;
