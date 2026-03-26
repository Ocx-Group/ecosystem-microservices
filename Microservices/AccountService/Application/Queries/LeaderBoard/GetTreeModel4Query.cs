using MediatR;

namespace Ecosystem.AccountService.Application.Queries.LeaderBoard;

public record GetTreeModel4Query(int? UserId) : IRequest<string?>;
