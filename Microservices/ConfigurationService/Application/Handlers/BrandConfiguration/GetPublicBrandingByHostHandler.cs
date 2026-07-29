using System.Diagnostics;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Observability;
using Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;
using Ecosystem.ConfigurationService.Application.Services;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public sealed class GetPublicBrandingByHostHandler
    : IRequestHandler<GetPublicBrandingByHostQuery, PublicBrandingDto?>
{
    private readonly IBrandConfigurationRepository _repository;
    private readonly ILogger<GetPublicBrandingByHostHandler> _logger;

    public GetPublicBrandingByHostHandler(
        IBrandConfigurationRepository repository,
        ILogger<GetPublicBrandingByHostHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PublicBrandingDto?> Handle(
        GetPublicBrandingByHostQuery request,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var normalizedHost = PublicBrandingResolver.NormalizeHost(request.Host);
            PublicBrandingResolution resolution;
            if (normalizedHost is null)
            {
                resolution = new PublicBrandingResolution(
                    PublicBrandingResolutionStatus.InvalidHost,
                    null,
                    null,
                    0);
            }
            else
            {
                var configurations = await _repository.GetAllAsync();
                resolution = PublicBrandingResolver.ResolveNormalizedHost(
                    normalizedHost,
                    configurations);
            }

            BrandingTelemetry.RecordResolution(
                resolution.Status,
                stopwatch.Elapsed.TotalMilliseconds);
            LogResolution(resolution, stopwatch.Elapsed.TotalMilliseconds);
            return resolution.Branding;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            BrandingTelemetry.RecordError(stopwatch.Elapsed.TotalMilliseconds);
            _logger.LogError(
                exception,
                "Public branding resolution failed after {ElapsedMilliseconds} ms",
                stopwatch.Elapsed.TotalMilliseconds);
            throw;
        }
    }

    private void LogResolution(
        PublicBrandingResolution resolution,
        double elapsedMilliseconds)
    {
        switch (resolution.Status)
        {
            case PublicBrandingResolutionStatus.Resolved:
                _logger.LogInformation(
                    "Public branding resolved for {Host} as BrandId {BrandId} in {ElapsedMilliseconds} ms",
                    resolution.NormalizedHost,
                    resolution.Branding!.BrandId,
                    elapsedMilliseconds);
                break;
            case PublicBrandingResolutionStatus.Ambiguous:
                _logger.LogError(
                    "Public branding is ambiguous for {Host}; at least {MatchCount} active configurations match",
                    resolution.NormalizedHost,
                    resolution.MatchCount);
                break;
            case PublicBrandingResolutionStatus.NotFound:
                _logger.LogInformation(
                    "Public branding was not found for {Host}",
                    resolution.NormalizedHost);
                break;
            case PublicBrandingResolutionStatus.InvalidHost:
                _logger.LogInformation("Public branding request contained an invalid host");
                break;
        }
    }
}
