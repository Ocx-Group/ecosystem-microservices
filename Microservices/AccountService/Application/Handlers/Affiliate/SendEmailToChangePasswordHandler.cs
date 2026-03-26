using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.Helpers;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.Bus;
using Ecosystem.Domain.Core.Events;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class SendEmailToChangePasswordHandler : IRequestHandler<SendEmailToChangePasswordCommand, bool>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IEventBus _eventBus;

    public SendEmailToChangePasswordHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IEventBus eventBus)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _eventBus = eventBus;
    }

    public async Task<bool> Handle(SendEmailToChangePasswordCommand request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByEmailAsync(request.Email.ToLower(), _tenantContext.TenantId);
        if (user is null) return false;

        user.VerificationCode = Guid.NewGuid().ToString();
        await _repo.UpdateAffiliateAsync(user);

        await _eventBus.Publish(new SendEmailEvent(
            "password_recovery",
            _tenantContext.TenantId,
            user.Email,
            user.Username,
            new Dictionary<string, string>
            {
                { "userName", user.Username },
                { "verificationCode", user.VerificationCode }
            }));

        return true;
    }
}
