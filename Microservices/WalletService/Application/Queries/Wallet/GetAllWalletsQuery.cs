using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Wallet;

public record GetAllWalletsQuery : IRequest<IEnumerable<WalletDto>>;
