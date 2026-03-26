using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Domain.Constants;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class EmailConfirmationHandler : IRequestHandler<EmailConfirmationCommand, bool>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public EmailConfirmationHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<bool> Handle(EmailConfirmationCommand request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByUserNameAsync(request.UserName, _tenantContext.TenantId);
        if (user is null) return false;

        user.StatusActivation = AccountServiceConstants.AffiliateStatus.Identificación_Pendiente.ToString();
        user.EmailVerification = true;
        await _repo.UpdateAffiliateAsync(user);
        return true;
    }
}
