using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletHistory;

public record DeleteWalletHistoryCommand(int Id) : IRequest<WalletHistoryDto?>;
