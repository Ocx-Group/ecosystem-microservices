using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecosystem.WalletService.Api.Controllers;

/// <summary>
/// Monthly commission liquidation, driven from admin/calculate-commissions.
///
/// Unlike the rest of WalletService this controller requires an admin JWT: it credits
/// wallets, and the brand it credits is taken from the token's brand_id claim, so one
/// brand's administrator cannot liquidate another's.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Policy = "BrandAdministrator")]
public class MonthlyCommissionController : BaseController
{
    private readonly IMediator _mediator;
    public MonthlyCommissionController(IMediator mediator) => _mediator = mediator;

    [HttpPost("calculate")]
    public async Task<IActionResult> Calculate(
        [FromBody] CalculateMonthlyCommissionsRequest request,
        CancellationToken ct)
    {
        var actorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
        var actorUserName = User.Identity?.Name ?? "unknown";

        var result = await _mediator.Send(
            new CalculateMonthlyCommissionsCommand(request, actorUserId, actorUserName),
            ct);

        return result.Status switch
        {
            CalculateMonthlyCommissionsStatus.Completed => Ok(Success(result.Result!)),
            CalculateMonthlyCommissionsStatus.Disabled => BadRequest(
                Fail(result.ValidationMessage ?? "The monthly commission is disabled for this brand")),
            CalculateMonthlyCommissionsStatus.InvalidRequest => BadRequest(
                Fail(result.ValidationMessage ?? "The liquidation parameters are invalid")),
            _ => NotFound(Fail("Brand configuration not found for the authenticated tenant"))
        };
    }
}
