using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public sealed class GetOwnBrandingHandler(
    IBrandConfigurationRepository repository,
    ITenantContext tenantContext)
    : IRequestHandler<GetOwnBrandingQuery, BrandingAdministrationDto?>
{
    public async Task<BrandingAdministrationDto?> Handle(
        GetOwnBrandingQuery request,
        CancellationToken cancellationToken)
    {
        var brandId = RequireTenant(tenantContext);
        var entity = await repository.GetByBrandIdAsync(brandId);
        return entity is null ? null : Map(entity);
    }

    internal static BrandingAdministrationDto Map(
        Domain.Models.BrandConfiguration entity) => new()
    {
        BrandId = entity.BrandId,
        Name = entity.Brand?.Name ?? string.Empty,
        CompanyName = entity.CompanyName,
        CompanyIdentifier = entity.CompanyIdentifier,
        ClientUrl = entity.ClientUrl,
        SupportEmail = entity.SupportEmail,
        SupportPhone = entity.SupportPhone,
        DocumentType = entity.DocumentType,
        LogoUrl = entity.LogoUrl,
        PrimaryColor = entity.PrimaryColor,
        SecondaryColor = entity.SecondaryColor,
        BackgroundColor = entity.BackgroundColor,
        UpdatedAt = entity.UpdatedAt
    };

    internal static long RequireTenant(ITenantContext context)
        => context.TenantId > 0
            ? context.TenantId
            : throw new UnauthorizedAccessException("Authenticated brand context is required.");
}
