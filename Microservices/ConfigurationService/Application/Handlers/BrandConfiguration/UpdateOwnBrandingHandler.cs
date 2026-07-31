using Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;
using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public sealed class UpdateOwnBrandingHandler(
    IBrandConfigurationRepository repository,
    IBrandConfigurationProvider brandConfigurationProvider,
    ITenantContext tenantContext,
    ILogger<UpdateOwnBrandingHandler> logger)
    : IRequestHandler<UpdateOwnBrandingCommand, BrandingAdministrationDto?>
{
    public async Task<BrandingAdministrationDto?> Handle(
        UpdateOwnBrandingCommand request,
        CancellationToken cancellationToken)
    {
        var brandId = GetOwnBrandingHandler.RequireTenant(tenantContext);
        var values = new Domain.Models.BrandConfiguration
        {
            BrandId = brandId,
            Brand = new Brand { Id = brandId, Name = request.Branding.Name.Trim() },
            CompanyName = request.Branding.CompanyName.Trim(),
            CompanyIdentifier = NormalizeOptional(request.Branding.CompanyIdentifier),
            ClientUrl = request.Branding.ClientUrl.Trim(),
            SupportEmail = request.Branding.SupportEmail.Trim(),
            SupportPhone = NormalizeOptional(request.Branding.SupportPhone),
            DocumentType = NormalizeOptional(request.Branding.DocumentType),
            LogoUrl = NormalizeOptional(request.Branding.LogoUrl),
            PrimaryColor = request.Branding.PrimaryColor.ToUpperInvariant(),
            SecondaryColor = request.Branding.SecondaryColor.ToUpperInvariant(),
            BackgroundColor = request.Branding.BackgroundColor.ToUpperInvariant()
        };

        var saved = await repository.UpdateBrandingAsync(brandId, values);
        if (saved is null)
            return null;

        await brandConfigurationProvider.InvalidateCacheAsync(brandId);

        logger.LogInformation(
            "Branding updated for BrandId {BrandId} by admin user {ActorUserId} ({ActorUserName})",
            brandId,
            request.ActorUserId,
            request.ActorUserName);

        return GetOwnBrandingHandler.Map(saved);
    }

    private static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
