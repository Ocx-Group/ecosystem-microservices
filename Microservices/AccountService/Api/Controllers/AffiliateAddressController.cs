using Asp.Versioning;
using Ecosystem.AccountService.Application.Commands.AffiliateAddress;
using Ecosystem.AccountService.Application.Queries.AffiliateAddress;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AffiliateAddressController : BaseController
{
    private readonly IMediator _mediator;

    public AffiliateAddressController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllAffiliateAddresses(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllAffiliateAddressesQuery(), ct);
        return result is null ? Ok(Fail("Affiliate addresses not found")) : Ok(Success(result));
    }

    [HttpGet("getAffiliateAddressByAffiliateId/{id:int}")]
    public async Task<IActionResult> GetAffiliateAddressByAffiliateId([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAffiliateAddressByAffiliateIdQuery(id), ct);
        return result is null ? Ok(Fail("Affiliate addresses not found")) : Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAffiliateAddress([FromBody] CreateAffiliateAddressCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is null ? Ok(Fail("The affiliate address wasn't created")) : Ok(Success(result));
    }
}
