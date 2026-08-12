using System.Text.Json;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public sealed class GetOwnCommissionSettingsHandler(
    IBrandConfigurationRepository repository,
    ITenantContext tenantContext)
    : IRequestHandler<GetOwnCommissionSettingsQuery, CommissionSettingsDto?>
{
    public async Task<CommissionSettingsDto?> Handle(
        GetOwnCommissionSettingsQuery request,
        CancellationToken cancellationToken)
    {
        var brandId = GetOwnBrandingHandler.RequireTenant(tenantContext);
        var entity = await repository.GetByBrandIdAsync(brandId);
        return entity is null ? null : Map(entity);
    }

    internal static CommissionSettingsDto Map(
        Domain.Models.BrandConfiguration entity) => new()
    {
        BrandId = entity.BrandId,
        CommissionEnabled = entity.CommissionEnabled,
        CommissionLevels = DeserializeLevels(entity.CommissionLevelsJson),
        DailyBonusAlwaysDistribute = entity.DailyBonusAlwaysDistribute,
        UpdatedAt = entity.UpdatedAt
    };

    /// <summary>
    /// The column is operator-editable jsonb, so malformed content is treated as
    /// "no levels configured" instead of faulting the whole dashboard page.
    /// </summary>
    internal static decimal[] DeserializeLevels(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return [];

        try
        {
            return JsonSerializer.Deserialize<decimal[]>(json) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }
}
