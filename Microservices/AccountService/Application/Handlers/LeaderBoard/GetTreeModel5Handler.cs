using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.LeaderBoard;
using Ecosystem.AccountService.Application.Queries.LeaderBoard;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.LeaderBoard;

public class GetTreeModel5Handler : IRequestHandler<GetTreeModel5Query, UserUniLevelTreeDto?>
{
    private readonly ILeaderBoardModel5Repository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTreeModel5Handler> _logger;

    public GetTreeModel5Handler(
        ILeaderBoardModel5Repository repository,
        IMapper mapper,
        ILogger<GetTreeModel5Handler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserUniLevelTreeDto?> Handle(GetTreeModel5Query request, CancellationToken cancellationToken)
    {
        var tree = await _repository.TreeModel5ByUser(request.UserId);

        if (tree is null || tree.Count == 0)
            return null;

        var mapped = _mapper.Map<List<UserUniLevelTreeDto>>(tree);
        return ConvertListToTreeModel5(mapped, request.UserId);
    }

    private static UserUniLevelTreeDto ConvertListToTreeModel5(List<UserUniLevelTreeDto> list, int userId)
    {
        var lookup = list.ToLookup(x => x.father);

        foreach (var item in list)
            item.children = lookup[item.id].ToList();

        return list.First(x => x.id == userId);
    }
}
