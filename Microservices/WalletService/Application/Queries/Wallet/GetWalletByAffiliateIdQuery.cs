using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Wallet;

public record GetWalletByAffiliateIdQuery(int AffiliateId) : IRequest<IEnumerable<WalletDto?>>;
