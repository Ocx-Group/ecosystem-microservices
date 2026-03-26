using MediatR;

namespace Ecosystem.AccountService.Application.Commands.LeaderBoard;

public record DeleteLeaderBoardModel6Command() : IRequest<string>;
