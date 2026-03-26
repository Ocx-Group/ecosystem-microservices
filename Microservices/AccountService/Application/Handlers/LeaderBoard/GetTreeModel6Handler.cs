using Ecosystem.AccountService.Application.Queries.LeaderBoard;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.LeaderBoard;

public class GetTreeModel6Handler : IRequestHandler<GetTreeModel6Query, List<ModelsFamilyTree>>
{
    private readonly ILeaderBoardModel6Repository _repository;
    private readonly ILogger<GetTreeModel6Handler> _logger;

    public GetTreeModel6Handler(
        ILeaderBoardModel6Repository repository,
        ILogger<GetTreeModel6Handler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<ModelsFamilyTree>> Handle(GetTreeModel6Query request, CancellationToken cancellationToken)
    {
        return await _repository.GetTreeModel6ByUser(request.UserId);
    }
}
