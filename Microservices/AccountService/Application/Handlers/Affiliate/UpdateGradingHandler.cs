using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class UpdateGradingHandler : IRequestHandler<UpdateGradingCommand, bool>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public UpdateGradingHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<bool> Handle(UpdateGradingCommand request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByIdAsync(request.UserId, _tenantContext.TenantId);
        if (user is null) return false;

        user.ExternalGradingIdBefore = request.GradingId;
        await _repo.UpdateAffiliateAsync(user);
        return true;
    }
}
