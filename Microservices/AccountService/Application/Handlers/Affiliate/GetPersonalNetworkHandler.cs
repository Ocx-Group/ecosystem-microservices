using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetPersonalNetworkHandler : IRequestHandler<GetPersonalNetworkQuery, List<AffiliatePersonalNetwork>>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public GetPersonalNetworkHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<List<AffiliatePersonalNetwork>> Handle(GetPersonalNetworkQuery request, CancellationToken ct)
    {
        var network = await _repo.GetPersonalNetwork(request.UserId);

        if (network.Count == 0)
            return network;

        // account_service.get_personal_network no devuelve el telefono. Se completa
        // aparte contra users_affiliates para no depender de un cambio en la funcion.
        var ids = network.Select(x => x.Id).Distinct().ToArray();
        var affiliates = await _repo.GetAffiliatesByIds(ids, _tenantContext.TenantId);

        var phoneById = affiliates
            .GroupBy(x => x.Id)
            .ToDictionary(g => g.Key, g => g.First().Phone);

        foreach (var item in network)
        {
            if (phoneById.TryGetValue(item.Id, out var phone))
                item.Phone = phone;
        }

        SetNetworkLevels(network, request.UserId);

        return network;
    }

    private static void SetNetworkLevels(List<AffiliatePersonalNetwork> network, int rootUserId)
    {
        var childrenByFather = network
            .GroupBy(item => (long)item.Father)
            .ToDictionary(group => group.Key, group => group.ToList());
        var visited = new HashSet<long>();
        var pending = new Queue<(long AffiliateId, int Level)>();
        pending.Enqueue((rootUserId, 0));

        while (pending.Count > 0)
        {
            var (affiliateId, level) = pending.Dequeue();

            if (!childrenByFather.TryGetValue(affiliateId, out var children))
                continue;

            foreach (var child in children)
            {
                if (!visited.Add(child.Id))
                    continue;

                child.Level = level + 1;
                pending.Enqueue((child.Id, child.Level));
            }
        }

        // La funcion de base de datos deberia devolver una red conectada al usuario
        // consultado. Si hubiera datos historicos huerfanos, se mantiene el contrato
        // de niveles positivos sin impedir que aparezcan en la respuesta.
        foreach (var item in network.Where(item => item.Level == 0))
            item.Level = 1;
    }
}
