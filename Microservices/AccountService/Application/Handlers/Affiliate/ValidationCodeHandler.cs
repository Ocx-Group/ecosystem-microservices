using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.Helpers;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class ValidationCodeHandler : IRequestHandler<ValidationCodeCommand, bool>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly IMasterPasswordRepository _masterPasswordRepo;
    private readonly ITenantContext _tenantContext;

    public ValidationCodeHandler(
        IUserAffiliateInfoRepository repo,
        IMasterPasswordRepository masterPasswordRepo,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _masterPasswordRepo = masterPasswordRepo;
        _tenantContext = tenantContext;
    }

    public async Task<bool> Handle(ValidationCodeCommand request, CancellationToken ct)
    {
        var affiliate = await _repo.GetAffiliateByIdAsync(request.UserId, _tenantContext.TenantId);
        if (affiliate is null) return false;

        var codeIsValid = request.Code == (affiliate.VerificationCode ?? string.Empty);
        var userPassIsValid = PasswordHelper.VerifyPassword(affiliate.Password, request.Password);

        var masterPassword = await _masterPasswordRepo.GetMasterPasswordByBrandId((int)_tenantContext.TenantId);
        var masterPassIsValid = masterPassword is not null &&
                                PasswordHelper.VerifyPassword(masterPassword.Password, request.Password);

        return codeIsValid && (userPassIsValid || masterPassIsValid);
    }
}
