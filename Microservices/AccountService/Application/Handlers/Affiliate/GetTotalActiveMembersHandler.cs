using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetTotalActiveMembersHandler : IRequestHandler<GetTotalActiveMembersQuery, int>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public GetTotalActiveMembersHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public Task<int> Handle(GetTotalActiveMembersQuery request, CancellationToken ct)
        => _repo.GetTotalActiveMembers(_tenantContext.TenantId);
}
