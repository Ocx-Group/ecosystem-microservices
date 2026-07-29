using System.Diagnostics;
using System.Diagnostics.Metrics;
using Ecosystem.ConfigurationService.Application.Services;

namespace Ecosystem.ConfigurationService.Application.Observability;

public static class BrandingTelemetry
{
    public const string MeterName = "Ecosystem.ConfigurationService.Branding";
    public const string MeterVersion = "1.0.0";

    private static readonly Meter Meter = new(MeterName, MeterVersion);
    private static readonly Counter<long> BootstrapRequests = Meter.CreateCounter<long>(
        "ecosystem.branding.bootstrap.requests",
        "{request}",
        "Public branding bootstrap requests grouped by outcome.");
    private static readonly Histogram<double> BootstrapDuration = Meter.CreateHistogram<double>(
        "ecosystem.branding.bootstrap.duration",
        "ms",
        "Time required to resolve public branding.");

    public static void RecordResolution(
        PublicBrandingResolutionStatus status,
        double elapsedMilliseconds)
    {
        var outcome = status switch
        {
            PublicBrandingResolutionStatus.Resolved => "resolved",
            PublicBrandingResolutionStatus.InvalidHost => "invalid_host",
            PublicBrandingResolutionStatus.NotFound => "not_found",
            PublicBrandingResolutionStatus.Ambiguous => "ambiguous",
            _ => "unknown"
        };
        var tags = new TagList { { "outcome", outcome } };
        BootstrapRequests.Add(1, tags);
        BootstrapDuration.Record(elapsedMilliseconds, tags);
    }

    public static void RecordError(double elapsedMilliseconds)
    {
        var tags = new TagList { { "outcome", "error" } };
        BootstrapRequests.Add(1, tags);
        BootstrapDuration.Record(elapsedMilliseconds, tags);
    }
}
