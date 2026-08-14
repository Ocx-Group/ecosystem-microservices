using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public sealed class GetOwnMonthlyCommissionSettingsHandler(
    IBrandConfigurationRepository repository,
    ITenantContext tenantContext)
    : IRequestHandler<GetOwnMonthlyCommissionSettingsQuery, MonthlyCommissionSettingsDto?>
{
    public async Task<MonthlyCommissionSettingsDto?> Handle(
        GetOwnMonthlyCommissionSettingsQuery request,
        CancellationToken cancellationToken)
    {
        var brandId = GetOwnBrandingHandler.RequireTenant(tenantContext);
        var entity = await repository.GetByBrandIdAsync(brandId);
        return entity is null ? null : Map(entity);
    }

    internal static MonthlyCommissionSettingsDto Map(
        Domain.Models.BrandConfiguration entity) => new()
    {
        BrandId = entity.BrandId,
        Enabled = entity.MonthlyCommissionEnabled,
        InterestRate = entity.MonthlyCommissionInterestRate,
        WaitingDays = entity.MonthlyCommissionWaitingDays,
        PaymentGroupId = entity.MonthlyCommissionPaymentGroupId,
        Source = entity.MonthlyCommissionSource,
        UpdatedAt = entity.UpdatedAt
    };
}
