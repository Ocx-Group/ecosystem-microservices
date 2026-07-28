using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletPeriod;

public record IsWithdrawalDateAllowedQuery(long BrandId) : IRequest<bool>;
