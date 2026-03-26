using Ecosystem.AccountService.Application.DTOs.LeaderBoard;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.LeaderBoard;

public record GetTreeModel5Query(int UserId) : IRequest<UserUniLevelTreeDto?>;
