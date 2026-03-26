using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Application.Settings;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Options;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetUrlSignUpHandler : IRequestHandler<GetUrlSignUpQuery, string?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly AccountServiceSettings _settings;

    public GetUrlSignUpHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IOptions<AccountServiceSettings> settings)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _settings = settings.Value;
    }

    public async Task<string?> Handle(GetUrlSignUpQuery request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByIdAsync(request.UserId, _tenantContext.TenantId);
        return user is null ? null : $"{_settings.ClientUrl}#/signup/{user.Username}";
    }
}
