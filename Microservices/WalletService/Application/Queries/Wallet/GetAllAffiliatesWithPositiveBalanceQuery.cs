using Ecosystem.WalletService.Domain.CustomModels;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Wallet;

public record GetAllAffiliatesWithPositiveBalanceQuery : IRequest<IEnumerable<AffiliateBalance>>;
