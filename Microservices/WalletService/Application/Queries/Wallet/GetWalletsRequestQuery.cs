using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Wallet;

public record GetWalletsRequestQuery(int UserId) : IRequest<IEnumerable<WalletDto>>;
