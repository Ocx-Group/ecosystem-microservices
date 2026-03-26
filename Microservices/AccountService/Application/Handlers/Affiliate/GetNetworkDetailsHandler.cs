using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetNetworkDetailsHandler : IRequestHandler<GetNetworkDetailsQuery, NetworkDetailsDto>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ILeaderBoardModel4Repository _model4Repo;
    private readonly ILeaderBoardModel5Repository _model5Repo;
    private readonly ILeaderBoardModel6Repository _model6Repo;
    private readonly ITenantContext _tenantContext;

    public GetNetworkDetailsHandler(
        IUserAffiliateInfoRepository repo,
        ILeaderBoardModel4Repository model4Repo,
        ILeaderBoardModel5Repository model5Repo,
        ILeaderBoardModel6Repository model6Repo,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _model4Repo = model4Repo;
        _model5Repo = model5Repo;
        _model6Repo = model6Repo;
        _tenantContext = tenantContext;
    }

    public async Task<NetworkDetailsDto> Handle(GetNetworkDetailsQuery request, CancellationToken ct)
    {
        var networkModel2 = await _repo.GetPersonalNetwork(request.AffiliateId);
        var directAffiliatesModel2 = await _repo.GetDirectAffiliatesCount(request.AffiliateId);

        var countLeft = 0;
        var countRight = 0;
        var firstLegLeft = await _model4Repo.GetChild(request.AffiliateId, 1);
        var firstLegRight = await _model4Repo.GetChild(request.AffiliateId, 2);
        var countDirectModel5 = await _model5Repo.CountDirectUsers(request.AffiliateId);
        var countInDirectModel5 = await _model5Repo.CountInDirectUsers();
        var countDirectModel6 = await _model6Repo.CountDirectUsers(request.AffiliateId);
        var countInDirectModel6 = await _model6Repo.CountInDirectUsers();

        if (firstLegLeft is not null)
        {
            var listUserLeft = await _model4Repo.GetTreeModel4ByUser(100, 0, firstLegLeft.AffiliateId);
            countLeft = listUserLeft.Count;
        }

        if (firstLegRight is not null)
        {
            var listUserRight = await _model4Repo.GetTreeModel4ByUser(100, 0, firstLegRight.AffiliateId);
            countRight = listUserRight.Count;
        }

        return new NetworkDetailsDto
        {
            Model123 = new StatisticsModel12356Dto
                { IndirectAffiliates = networkModel2.Count, DirectAffiliates = directAffiliatesModel2 },
            Model4 = new StatisticsModel4Dto { LeftCount = countLeft, RightCount = countRight },
            Model5 = new StatisticsModel12356Dto
            {
                IndirectAffiliates = countInDirectModel5 == 0 ? 0 : countInDirectModel5 - 1,
                DirectAffiliates = countDirectModel5
            },
            Model6 = new StatisticsModel12356Dto
            {
                IndirectAffiliates = countInDirectModel6 == 0 ? 0 : countInDirectModel6 - 1,
                DirectAffiliates = countDirectModel6
            }
        };
    }
}
