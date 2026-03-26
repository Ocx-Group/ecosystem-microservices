using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Matrix;
using Ecosystem.AccountService.Application.Queries.Matrix;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Matrix;

public class GetUniLevelFamilyTreeHandler : IRequestHandler<GetUniLevelFamilyTreeQuery, UserUniLevelTreeDto?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUniLevelFamilyTreeHandler> _logger;

    public GetUniLevelFamilyTreeHandler(
        IUserAffiliateInfoRepository repo,
        IMapper mapper,
        ILogger<GetUniLevelFamilyTreeHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserUniLevelTreeDto?> Handle(GetUniLevelFamilyTreeQuery request, CancellationToken cancellationToken)
    {
        const int maxLevels = 7;

        if (request.UserId is not null)
        {
            var userTree = await _repo.GetUniLevelFamilyTree(maxLevels, 0, 1, request.UserId.Value);

            if (userTree is null or { Count: 0 })
                return null;

            var listUser = _mapper.Map<ICollection<UserUniLevelTreeDto>>(userTree);
            var user = ConvertListToTreeUniLevel(listUser, request.UserId.Value, isAdmin: false);
            return user;
        }

        var childrenTree = await _repo.GetUniLevelFamilyTree(maxLevels, 1, 1);

        if (childrenTree is null or { Count: 0 })
            return null;

        var listChild = _mapper.Map<ICollection<UserUniLevelTreeDto>>(childrenTree);
        var child = ConvertListToTreeUniLevel(listChild, 0, isAdmin: true);

        var admin = new UserUniLevelTreeDto
        {
            userName = "Administrador",
            id = 0,
            children = new List<UserUniLevelTreeDto> { child }
        };

        return admin;
    }

    private static UserUniLevelTreeDto ConvertListToTreeUniLevel(ICollection<UserUniLevelTreeDto> list, int id, bool isAdmin)
    {
        var lookup = list.ToLookup(x => x.father);
        foreach (var item in list)
            item.children = lookup[item.id].ToList();

        return isAdmin ? list.First(x => x.father == 0) : list.First(x => x.id == id);
    }
}
