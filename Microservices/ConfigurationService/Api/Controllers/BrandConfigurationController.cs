using Asp.Versioning;
using Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecosystem.ConfigurationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BrandConfigurationController : BaseController
{
    private readonly IMediator _mediator;
    public BrandConfigurationController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Authorize(Policy = "BrandConfigurationFullAccess")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var brandId = GetAuthenticatedBrandId();
        var result = await _mediator.Send(new GetBrandConfigurationByBrandIdQuery(brandId), ct);
        return Ok(Success(result is null ? [] : new[] { result }));
    }

    [HttpGet("{brandId:long}")]
    [Authorize(Policy = "BrandConfigurationFullAccess")]
    public async Task<IActionResult> GetByBrandId([FromRoute] long brandId, CancellationToken ct)
    {
        if (brandId != GetAuthenticatedBrandId())
            return Forbid();

        var result = await _mediator.Send(new GetBrandConfigurationByBrandIdQuery(brandId), ct);
        return result is null
            ? NotFound(Fail($"Brand configuration not found for BrandId {brandId}"))
            : Ok(Success(result));
    }

    [HttpGet("public/current")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicCurrent(
        [FromQuery] string host,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(host))
            return BadRequest(Fail("Host is required"));

        var result = await _mediator.Send(new GetPublicBrandingByHostQuery(host), ct);
        if (result is null)
        {
            Response.Headers.CacheControl = "no-store";
            return NotFound(Fail("Active branding not found for the requested host"));
        }

        Response.Headers.CacheControl = "public, max-age=300";
        Response.Headers.Append("X-Branding-Contract", "public-branding-v1");
        return Ok(Success(result));
    }

    [HttpPut]
    [Authorize(Policy = "BrandConfigurationFullAccess")]
    public async Task<IActionResult> Upsert([FromBody] UpsertBrandConfigurationCommand command, CancellationToken ct)
    {
        if (command.BrandId != GetAuthenticatedBrandId())
            return Forbid();

        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpDelete("{brandId:long}")]
    [Authorize(Policy = "BrandConfigurationFullAccess")]
    public async Task<IActionResult> Delete([FromRoute] long brandId, CancellationToken ct)
    {
        if (brandId != GetAuthenticatedBrandId())
            return Forbid();

        var result = await _mediator.Send(new DeleteBrandConfigurationCommand(brandId), ct);
        return result is null
            ? NotFound(Fail($"Brand configuration not found for BrandId {brandId}"))
            : Ok(Success(result));
    }

    [HttpGet("admin/current")]
    [Authorize(Policy = "BrandAdministrator")]
    public async Task<IActionResult> GetOwnBranding(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOwnBrandingQuery(), ct);
        return result is null
            ? NotFound(Fail("Brand configuration not found for the authenticated tenant"))
            : Ok(Success(result));
    }

    [HttpPut("admin/current")]
    [Authorize(Policy = "BrandAdministrator")]
    public async Task<IActionResult> UpdateOwnBranding(
        [FromBody] UpdateOwnBrandingRequest request,
        CancellationToken ct)
    {
        var actorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
        var actorUserName = User.Identity?.Name ?? "unknown";
        var result = await _mediator.Send(
            new UpdateOwnBrandingCommand(request, actorUserId, actorUserName),
            ct);

        return result.Status switch
        {
            UpdateOwnBrandingStatus.Updated => Ok(Success(result.Branding!)),
            UpdateOwnBrandingStatus.InvalidHost => BadRequest(
                Fail("ClientUrl must contain a resolvable hostname")),
            UpdateOwnBrandingStatus.HostConflict => Conflict(
                Fail("ClientUrl already belongs to another active brand")),
            _ => NotFound(Fail("Brand configuration not found for the authenticated tenant"))
        };
    }

    [HttpGet("admin/commissions")]
    [Authorize(Policy = "BrandAdministrator")]
    public async Task<IActionResult> GetOwnCommissionSettings(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOwnCommissionSettingsQuery(), ct);
        return result is null
            ? NotFound(Fail("Brand configuration not found for the authenticated tenant"))
            : Ok(Success(result));
    }

    [HttpPut("admin/commissions")]
    [Authorize(Policy = "BrandAdministrator")]
    public async Task<IActionResult> UpdateOwnCommissionSettings(
        [FromBody] UpdateCommissionSettingsRequest request,
        CancellationToken ct)
    {
        var actorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
        var actorUserName = User.Identity?.Name ?? "unknown";
        var result = await _mediator.Send(
            new UpdateOwnCommissionSettingsCommand(request, actorUserId, actorUserName),
            ct);

        return result.Status switch
        {
            UpdateCommissionSettingsStatus.Updated => Ok(Success(result.Settings!)),
            UpdateCommissionSettingsStatus.InvalidLevels => BadRequest(
                Fail(result.ValidationMessage ?? "The commission levels are invalid")),
            _ => NotFound(Fail("Brand configuration not found for the authenticated tenant"))
        };
    }

    private long GetAuthenticatedBrandId()
        => long.TryParse(User.FindFirstValue("brand_id"), out var brandId) && brandId > 0
            ? brandId
            : throw new UnauthorizedAccessException("Authenticated brand context is required.");
}
