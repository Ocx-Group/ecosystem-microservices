using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Affiliate;

public record GetTotalActiveMembersQuery : IRequest<int>;
