using Ecosystem.AccountService.Application.Adapters;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.Helpers;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.Bus;
using Ecosystem.Domain.Core.Events;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GenerateVerificationCodeHandler : IRequestHandler<GenerateVerificationCodeCommand, bool>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IEventBus _eventBus;
    private readonly IWalletServiceAdapter _walletAdapter;

    public GenerateVerificationCodeHandler(
        IUserAffiliateInfoRepository repo,
        ITenantContext tenantContext,
        IEventBus eventBus,
        IWalletServiceAdapter walletAdapter)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _eventBus = eventBus;
        _walletAdapter = walletAdapter;
    }

    public async Task<bool> Handle(GenerateVerificationCodeCommand request, CancellationToken ct)
    {
        if (request.CheckDate)
        {
            var isDateValid = await _walletAdapter.IsWithdrawalDateAllowed(_tenantContext.TenantId);
            if (!isDateValid) return false;
        }

        var user = await _repo.GetAffiliateByIdAsync(request.Id, _tenantContext.TenantId);
        if (user is null) return false;

        user.VerificationCode = VerificationCodeHelper.GenerateVerificationCode();

        await _eventBus.Publish(new SendEmailEvent(
            "verification_code",
            _tenantContext.TenantId,
            user.Email,
            user.Username,
            new Dictionary<string, string>
            {
                { "userName", user.Username },
                { "verificationCode", user.VerificationCode }
            }));

        await _repo.UpdateVerificationCodeAffiliateAsync(user);
        return true;
    }
}
