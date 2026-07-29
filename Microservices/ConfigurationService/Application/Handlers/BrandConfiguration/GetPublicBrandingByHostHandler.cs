using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public sealed class GetPublicBrandingByHostHandler
    : IRequestHandler<GetPublicBrandingByHostQuery, PublicBrandingDto?>
{
    private readonly IBrandConfigurationRepository _repository;

    public GetPublicBrandingByHostHandler(IBrandConfigurationRepository repository)
        => _repository = repository;

    public async Task<PublicBrandingDto?> Handle(
        GetPublicBrandingByHostQuery request,
        CancellationToken cancellationToken)
    {
        var requestedHost = NormalizeHost(request.Host);
        if (requestedHost is null) return null;

        var configurations = await _repository.GetAllAsync();
        var matches = configurations
            .Where(configuration =>
                configuration.IsActive &&
                configuration.DeletedAt is null &&
                configuration.Brand is { IsActive: not false, DeletedAt: null } &&
                string.Equals(
                    NormalizeHost(configuration.ClientUrl),
                    requestedHost,
                    StringComparison.OrdinalIgnoreCase))
            .Take(2)
            .ToList();

        // A hostname must resolve to exactly one active brand. Never choose a
        // tenant silently when the central configuration is ambiguous.
        if (matches.Count != 1) return null;
        var entity = matches[0];

        return new PublicBrandingDto
        {
            BrandId = entity.BrandId,
            // X-Client-ID identifies the tenant but does not authorize a
            // request; protected APIs still require their authorization token.
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
        };
    }

    private static string? NormalizeHost(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;

        var candidate = value.Trim();
        if (!candidate.Contains("://", StringComparison.Ordinal))
            candidate = $"https://{candidate}";

        if (!Uri.TryCreate(candidate, UriKind.Absolute, out var uri))
            return null;

        var host = uri.IdnHost.TrimEnd('.').ToLowerInvariant();
        return host.StartsWith("www.", StringComparison.Ordinal)
            ? host[4..]
            : host;
    }
}
