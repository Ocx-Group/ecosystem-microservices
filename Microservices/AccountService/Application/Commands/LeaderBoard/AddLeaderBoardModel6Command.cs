using Ecosystem.AccountService.Domain.Models;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.LeaderBoard;

public record AddLeaderBoardModel6Command(List<LeaderBoardModel6> LeaderBoard) : IRequest<string>;
