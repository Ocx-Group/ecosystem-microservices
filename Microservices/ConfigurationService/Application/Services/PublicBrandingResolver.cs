using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Application.Services;

public enum PublicBrandingResolutionStatus
{
    Resolved,
    InvalidHost,
    NotFound,
    Ambiguous
}

public sealed record PublicBrandingResolution(
    PublicBrandingResolutionStatus Status,
    string? NormalizedHost,
    PublicBrandingDto? Branding,
    int MatchCount);

/// <summary>
/// Pure domain-to-public-contract resolver. Keeping this logic outside the
/// database adapter makes the hostname contract testable without a database.
/// </summary>
public static class PublicBrandingResolver
{
    public static PublicBrandingResolution Resolve(
        string? requestedHost,
        IEnumerable<BrandConfiguration> configurations)
    {
        var normalizedHost = NormalizeHost(requestedHost);
        return normalizedHost is null
            ? new PublicBrandingResolution(
                PublicBrandingResolutionStatus.InvalidHost,
                null,
                null,
                0)
            : ResolveNormalizedHost(normalizedHost, configurations);
    }

    public static PublicBrandingResolution ResolveNormalizedHost(
        string normalizedHost,
        IEnumerable<BrandConfiguration> configurations)
    {
        var matches = configurations
            .Where(configuration =>
                configuration.IsActive &&
                configuration.DeletedAt is null &&
                configuration.Brand is { IsActive: not false, DeletedAt: null } &&
                string.Equals(
                    NormalizeHost(configuration.ClientUrl),
                    normalizedHost,
                    StringComparison.OrdinalIgnoreCase))
            .Take(2)
            .ToList();

        if (matches.Count == 0)
        {
            return new PublicBrandingResolution(
                PublicBrandingResolutionStatus.NotFound,
                normalizedHost,
                null,
                0);
        }

        // A hostname must resolve to exactly one active brand. Never select a
        // tenant silently when the central configuration is ambiguous.
        if (matches.Count != 1)
        {
            return new PublicBrandingResolution(
                PublicBrandingResolutionStatus.Ambiguous,
                normalizedHost,
                null,
                matches.Count);
        }

        var entity = matches[0];
        return new PublicBrandingResolution(
            PublicBrandingResolutionStatus.Resolved,
            normalizedHost,
            new PublicBrandingDto
            {
                BrandId = entity.BrandId,
                // X-Client-ID identifies the tenant but does not authorize a
                // request; protected APIs still require authorization.
                ClientId = entity.Brand.SecretKey,
                Name = entity.Brand.Name,
                CompanyName = entity.CompanyName,
                ClientUrl = entity.ClientUrl,
                SupportEmail = entity.SupportEmail,
                SupportPhone = entity.SupportPhone,
                DocumentType = entity.DocumentType,
                LogoUrl = entity.LogoUrl,
                PrimaryColor = entity.PrimaryColor,
                SecondaryColor = entity.SecondaryColor,
                BackgroundColor = entity.BackgroundColor
            },
            1);
    }

    public static string? NormalizeHost(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;

        var candidate = value.Trim();
        if (!candidate.Contains("://", StringComparison.Ordinal))
            candidate = $"https://{candidate}";

        if (!Uri.TryCreate(candidate, UriKind.Absolute, out var uri))
            return null;

        var host = uri.IdnHost.TrimEnd('.').ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(host)) return null;

        return host.StartsWith("www.", StringComparison.Ordinal)
            ? host[4..]
            : host;
    }
}
