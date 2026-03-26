using Ecosystem.AccountService.Application.Commands.LeaderBoard;
using Ecosystem.AccountService.Application.DTOs.LeaderBoard;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.LeaderBoard;

public class CalculateResultLeaderBoardHandler : IRequestHandler<CalculateResultLeaderBoardCommand, ICollection<LeaderBoardResultModel4Dto>>
{
    private readonly ILeaderBoardModel4Repository _repository;
    private readonly ILogger<CalculateResultLeaderBoardHandler> _logger;

    public CalculateResultLeaderBoardHandler(
        ILeaderBoardModel4Repository repository,
        ILogger<CalculateResultLeaderBoardHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ICollection<LeaderBoardResultModel4Dto>> Handle(
        CalculateResultLeaderBoardCommand request,
        CancellationToken cancellationToken)
    {
        var results = new List<LeaderBoardResultModel4Dto>();

        foreach (var node in request.VolumeData)
        {
            decimal leftVolume = 0;
            decimal rightVolume = 0;

            var leftChild = await _repository.GetChild(node.Key, 1);
            var rightChild = await _repository.GetChild(node.Key, 2);

            if (leftChild is not null)
            {
                var leftTree = await _repository.GetTreeModel4ByUser(100, 0, leftChild.AffiliateId);
                leftVolume = leftTree
                    .Where(x => request.VolumeData.ContainsKey(x.Id))
                    .Sum(x => request.VolumeData[x.Id]);
            }

            if (rightChild is not null)
            {
                var rightTree = await _repository.GetTreeModel4ByUser(100, 0, rightChild.AffiliateId);
                rightVolume = rightTree
                    .Where(x => request.VolumeData.ContainsKey(x.Id))
                    .Sum(x => request.VolumeData[x.Id]);
            }

            results.Add(new LeaderBoardResultModel4Dto
            {
                AffiliateId = node.Key,
                LeftVolume = leftVolume,
                RightVolume = rightVolume
            });
        }

        return results;
    }
}
