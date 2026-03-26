using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Domain.Constants;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class AuthorizationAffiliatesHandler : IRequestHandler<AuthorizationAffiliatesCommand, bool>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AuthorizationAffiliatesHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<bool> Handle(AuthorizationAffiliatesCommand request, CancellationToken ct)
    {
        var approvedUserList = new List<Domain.Models.UsersAffiliate>();
        var disApprovedUserList = new List<Domain.Models.UsersAffiliate>();
        var today = DateTime.Now;

        if (request.ApprovedArray.Length is not 0)
        {
            approvedUserList = await _repo.GetAffiliatesByIds(request.ApprovedArray, _tenantContext.TenantId);
            Parallel.ForEach(approvedUserList, item =>
            {
                item.CardIdAuthorization = true;
                item.CardIdMessage = AccountServiceConstants.CardIdStatus.Aprobado.ToString();
                item.UpdatedAt = today;
            });
        }

        if (request.DisApprovedArray.Length is not 0)
        {
            disApprovedUserList = await _repo.GetAffiliatesByIds(request.DisApprovedArray, _tenantContext.TenantId);
            Parallel.ForEach(disApprovedUserList, item =>
            {
                item.CardIdAuthorization = false;
                item.CardIdMessage = AccountServiceConstants.CardIdStatus.Rechazado.ToString();
                item.DeletedAt = today;
                item.UpdatedAt = today;
            });
        }

        await _repo.UpdateBulkAffiliateAsync(approvedUserList.Union(disApprovedUserList).ToList());
        return true;
    }
}
