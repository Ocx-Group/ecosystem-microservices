using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Invoice;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.Bus;
using Ecosystem.Domain.Core.Events;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class SendCourseInvitationsHandler : IRequestHandler<SendCourseInvitationsCommand, IEnumerable<UserAffiliateResponse>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IEventBus _eventBus;
    private readonly ITenantContext _tenantContext;

    public SendCourseInvitationsHandler(
        IInvoiceRepository invoiceRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IEventBus eventBus,
        ITenantContext tenantContext,
        ILogger<SendCourseInvitationsHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _eventBus = eventBus;
        _tenantContext = tenantContext;
    }

    public async Task<IEnumerable<UserAffiliateResponse>> Handle(SendCourseInvitationsCommand command, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var allInvoices = await _invoiceRepository.GetAllInvoicesForTradingAcademyPurchases();
        var nextMonday = CommonExtensions.CalculateNextMonday(DateTime.Today);

        if (allInvoices is null)
            return new List<UserAffiliateResponse>();

        var tasks = allInvoices.Where(invoice =>
                invoice.ProductId is Constants.ForMonth or Constants.ForWeek)
            .Select(async invoice =>
            {
                var endDate = invoice.ProductId == Constants.ForMonth
                    ? CommonExtensions.CalculateMonthlyCourseDates(invoice.CreatedAt).EndDate
                    : CommonExtensions.CalculateWeeklyCourseDates(invoice.CreatedAt).EndDate;

                if (endDate.Date < nextMonday)
                    return null;

                var userResponse = await _accountServiceAdapter.GetAffiliateByUserName(invoice.UserName, brandId);
                if (userResponse is null)
                    return null;

                await _eventBus.Publish(new SendEmailEvent(
                    templateKey: Constants.SubjectInvitationForAcademy,
                    brandId: brandId,
                    toEmail: userResponse.Email ?? string.Empty,
                    toName: userResponse.Name ?? string.Empty,
                    placeholders: new Dictionary<string, string>
                    {
                        { "link", command.Link },
                        { "code", command.Code },
                        { "name", userResponse.Name ?? string.Empty }
                    }
                ));

                return new UserAffiliateResponse { Success = true, Data = userResponse };
            });

        var results = await Task.WhenAll(tasks);
        return results.Where(user => user != null).Select(user => user!);
    }
}
