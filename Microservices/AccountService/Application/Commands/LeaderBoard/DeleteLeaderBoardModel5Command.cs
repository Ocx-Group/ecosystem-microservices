using MediatR;

namespace Ecosystem.AccountService.Application.Commands.LeaderBoard;

public record DeleteLeaderBoardModel5Command() : IRequest<string>;
