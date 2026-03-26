using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.LeaderBoard;
using Ecosystem.AccountService.Application.Queries.LeaderBoard;
using Ecosystem.AccountService.Domain.Constants;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ecosystem.AccountService.Application.Handlers.LeaderBoard;

public class GetTreeModel4Handler : IRequestHandler<GetTreeModel4Query, string?>
{
    private readonly ILeaderBoardModel4Repository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTreeModel4Handler> _logger;

    public GetTreeModel4Handler(
        ILeaderBoardModel4Repository repository,
        IMapper mapper,
        ILogger<GetTreeModel4Handler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string?> Handle(GetTreeModel4Query request, CancellationToken cancellationToken)
    {
        var maxLevels = AccountServiceConstants.EcoPoolLevels;

        if (request.UserId is not null)
        {
            var tree = await _repository.GetTreeModel4ByUser(maxLevels, 0, request.UserId.Value);

            if (tree is null || tree.Count == 0)
                return null;

            var mapped = _mapper.Map<List<UserBinaryTreeDto>>(tree);
            var root = ConvertListToTreeBinary(mapped, false, request.UserId.Value);

            return Serialize(root);
        }
        else
        {
            var tree = await _repository.GetTreeModel4ByUser(maxLevels, 1);

            if (tree is null || tree.Count == 0)
                return null;

            var mapped = _mapper.Map<List<UserBinaryTreeDto>>(tree);
            var child = ConvertListToTreeBinary(mapped, true, 0);

            var adminNode = new UserBinaryTreeDto
            {
                UserName = "administrador",
                Id = 0,
                Children = [child],
                Level = 0
            };

            return Serialize(adminNode);
        }
    }

    private static UserBinaryTreeDto ConvertListToTreeBinary(List<UserBinaryTreeDto> list, bool isAdmin, int id)
    {
        var lookup = list.ToLookup(x => x.Father);

        foreach (var item in list)
            if (lookup.Any(x => x.Key == item.Id))
                item.Children = lookup[item.Id].ToList();

        return isAdmin
            ? list.First(x => x.Father == 0)
            : list.First(x => x.Id == id);
    }

    private static string Serialize(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }
}
