using Asp.Versioning;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.NotificationService.Application.Adapters;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.NotificationService.Api.Controllers;

/// <summary>
/// Backward-compatible, read-only facade for existing template dashboards.
/// Configuration writes belong exclusively to ConfigurationService.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/email-sender-config")]
public class BrandConfigurationController : BaseController
{
    private readonly IBrandConfigurationReader _brandConfigurationReader;
    private readonly ITenantContext _tenantContext;

    public BrandConfigurationController(
        IBrandConfigurationReader brandConfigurationReader,
        ITenantContext tenantContext)
    {
        _brandConfigurationReader = brandConfigurationReader;
        _tenantContext = tenantContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        var configuration = await _brandConfigurationReader.GetByBrandIdAsync(
            _tenantContext.TenantId,
            cancellationToken);

        if (configuration is null)
            return Ok(Success(Array.Empty<EmailSenderConfigurationResponse>()));

        var response = new EmailSenderConfigurationResponse(
            configuration.BrandId,
            configuration.Name,
            configuration.SenderName,
            configuration.SenderEmail,
            configuration.SupportEmail,
            configuration.ClientUrl,
            true);

        return Ok(Success(new[] { response }));
    }

    public sealed record EmailSenderConfigurationResponse(
        long BrandId,
        string Name,
        string SenderName,
        string SenderEmail,
        string SupportEmail,
        string ClientUrl,
        bool IsActive);
}
