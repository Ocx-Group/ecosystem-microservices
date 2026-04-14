using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletPeriod;

public record IsWithdrawalDateAllowedQuery() : IRequest<bool>;
