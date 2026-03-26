using Ecosystem.AccountService.Application.DTOs.LeaderBoard;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.LeaderBoard;

public record CalculateResultLeaderBoardCommand(Dictionary<int, decimal> VolumeData) : IRequest<ICollection<LeaderBoardResultModel4Dto>>;
