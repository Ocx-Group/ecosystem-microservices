using Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;
using Ecosystem.ConfigurationService.Application.Services;
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
    : IRequestHandler<UpdateOwnBrandingCommand, UpdateOwnBrandingResult>
{
    public async Task<UpdateOwnBrandingResult> Handle(
        UpdateOwnBrandingCommand request,
        CancellationToken cancellationToken)
    {
        var brandId = GetOwnBrandingHandler.RequireTenant(tenantContext);

        var normalizedHost = PublicBrandingResolver.NormalizeHost(request.Branding.ClientUrl);
        if (normalizedHost is null)
        {
            logger.LogWarning(
                "Branding update for BrandId {BrandId} rejected: ClientUrl does not resolve to a hostname",
                brandId);
            return new UpdateOwnBrandingResult(UpdateOwnBrandingStatus.InvalidHost, null);
        }

        if (await HostBelongsToAnotherBrandAsync(brandId, normalizedHost))
        {
            logger.LogWarning(
                "Branding update for BrandId {BrandId} rejected: host {NormalizedHost} already belongs to another active brand",
                brandId,
                normalizedHost);
            return new UpdateOwnBrandingResult(UpdateOwnBrandingStatus.HostConflict, null);
        }

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
            return new UpdateOwnBrandingResult(UpdateOwnBrandingStatus.NotFound, null);

        await brandConfigurationProvider.InvalidateCacheAsync(brandId);

        logger.LogInformation(
            "Branding updated for BrandId {BrandId} by admin user {ActorUserId} ({ActorUserName})",
            brandId,
            request.ActorUserId,
            request.ActorUserName);

        return new UpdateOwnBrandingResult(
            UpdateOwnBrandingStatus.Updated,
            GetOwnBrandingHandler.Map(saved));
    }

    /// <summary>
    /// A hostname must resolve to exactly one active brand. Accepting a duplicate
    /// here would make <see cref="PublicBrandingResolver"/> answer
    /// <c>Ambiguous</c> for both brands, so their websites would silently start in
    /// fallback. The activity predicate mirrors the resolver on purpose.
    /// </summary>
    private async Task<bool> HostBelongsToAnotherBrandAsync(long brandId, string normalizedHost)
    {
        var configurations = await repository.GetAllAsync();

        return configurations.Any(configuration =>
            configuration.BrandId != brandId &&
            configuration.IsActive &&
            configuration.DeletedAt is null &&
            configuration.Brand is { IsActive: not false, DeletedAt: null } &&
            string.Equals(
                PublicBrandingResolver.NormalizeHost(configuration.ClientUrl),
                normalizedHost,
                StringComparison.OrdinalIgnoreCase));
    }

    private static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
