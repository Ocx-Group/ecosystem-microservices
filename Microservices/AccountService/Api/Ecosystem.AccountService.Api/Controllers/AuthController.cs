using Asp.Versioning;
using Ecosystem.AccountService.Application.Commands.Auth;
using Ecosystem.AccountService.Application.Queries.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> UserAuthentication(
        [FromBody] UserAuthenticationCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        if (!result.IsAuthenticated)
            return Ok(Fail("The user name or pass is incorrect!"));

        return Ok(Success(result.Affiliate is not null ? result.Affiliate : result.User!));
    }

    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries(CancellationToken ct)
    {
        var countries = await _mediator.Send(new GetCountriesQuery(), ct);
        return Ok(Success(countries));
    }

    [HttpGet("login_movements/{affiliateId}")]
    public async Task<IActionResult> GetLoginMovementsByAffiliateId(int affiliateId, CancellationToken ct)
    {
        var movements = await _mediator.Send(new GetLoginMovementsByAffiliateIdQuery(affiliateId), ct);
        return Ok(Success(movements));
    }

    [HttpPost("generate_hash")]
    public async Task<IActionResult> GenerateHash([FromBody] string password, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(password))
            return Ok(Fail("The password is required"));

        var hash = await _mediator.Send(new GenerateHashCommand(password), ct);
        return Ok(Success(new { Hash = hash }));
    }
}
