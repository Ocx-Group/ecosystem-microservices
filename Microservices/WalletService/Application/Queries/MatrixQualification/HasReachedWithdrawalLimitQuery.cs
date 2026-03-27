using MediatR;

namespace Ecosystem.WalletService.Application.Queries.MatrixQualification;

public record HasReachedWithdrawalLimitQuery(long UserId) : IRequest<bool>;
