using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.Bus;
using Ecosystem.Domain.Core.Events;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class SendEmailConfirmationHandler : IRequestHandler<SendEmailConfirmationCommand, bool>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IEventBus _eventBus;

    public SendEmailConfirmationHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext, IEventBus eventBus)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _eventBus = eventBus;
    }

    public async Task<bool> Handle(SendEmailConfirmationCommand request, CancellationToken ct)
    {
        var user = await _repo.GetAffiliateByIdAsync(request.Id, _tenantContext.TenantId);
        if (user is null) return false;

        await _eventBus.Publish(new SendEmailEvent(
            "email_validation",
            _tenantContext.TenantId,
            user.Email,
            user.Username,
            new Dictionary<string, string>
            {
                { "userName", user.Username },
                { "verificationCode", user.VerificationCode ?? "" }
            }));

        return true;
    }
}
