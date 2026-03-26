using Ecosystem.AccountService.Domain.Models;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.LeaderBoard;

public record AddLeaderBoardModel5Command(List<LeaderBoardModel5> LeaderBoard) : IRequest<string>;
