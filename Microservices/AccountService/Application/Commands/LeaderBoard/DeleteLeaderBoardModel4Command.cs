using MediatR;

namespace Ecosystem.AccountService.Application.Commands.LeaderBoard;

public record DeleteLeaderBoardModel4Command() : IRequest<string>;
