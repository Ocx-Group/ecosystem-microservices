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

        return network;
    }
}
