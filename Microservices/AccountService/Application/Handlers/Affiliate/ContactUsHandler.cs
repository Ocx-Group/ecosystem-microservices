using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.Domain.Core.Bus;
using Ecosystem.Domain.Core.Events;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class ContactUsHandler : IRequestHandler<ContactUsCommand, bool>
{
    private readonly ITenantContext _tenantContext;
    private readonly IEventBus _eventBus;

    public ContactUsHandler(ITenantContext tenantContext, IEventBus eventBus)
    {
        _tenantContext = tenantContext;
        _eventBus = eventBus;
    }

    public async Task<bool> Handle(ContactUsCommand request, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.Email))
            return false;

        await _eventBus.Publish(new SendEmailEvent(
            "contact_us",
            _tenantContext.TenantId,
            request.Email,
            request.FullName,
            new Dictionary<string, string>
            {
                { "fullName", request.FullName },
                { "email", request.Email },
                { "phoneNumber", request.PhoneNumber ?? "" },
                { "subject", request.Subject },
                { "message", request.Message }
            }));

        return true;
    }
}
