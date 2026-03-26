using Asp.Versioning;
using Ecosystem.AccountService.Application.Commands.AffiliateBtc;
using Ecosystem.AccountService.Application.Queries.AffiliateBtc;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AffiliateBtcController : BaseController
{
    private readonly IMediator _mediator;

    public AffiliateBtcController(IMediator mediator) => _mediator = mediator;

    [HttpGet("get_affiliate_btc_by_affiliate_id/{id:int}")]
    public async Task<IActionResult> GetAffiliateBtcByAffiliateId([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetAffiliateBtcByAffiliateIdQuery(id));

        return result is null
            ? Ok(Fail("The affiliate btc wasn't find"))
            : Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAffiliateBtc([FromBody] CreateAffiliateBtcCommand command)
    {
        var result = await _mediator.Send(command);

        return result.Count == 0
            ? Ok(Fail("The affiliate btc wasn't created."))
            : Ok(Success(result));
    }

    [HttpPut]
    public async Task<IActionResult> CreateAffiliateBtcMobile([FromBody] CreateAffiliateBtcCommand command)
    {
        var result = await _mediator.Send(command);

        return result.Count == 0
            ? Ok(Fail("The affiliate btc wasn't created."))
            : Ok(Success(result));
    }

    [HttpPost("get_affiliates_btc_by_ids")]
    public async Task<IActionResult> GetAffiliatesBtcByIds([FromBody] long[] ids)
    {
        var result = await _mediator.Send(new GetAffiliatesBtcByIdsQuery(ids));

        if (result is null || !result.Any())
            return Ok(Fail("No affiliate btc found for the provided Ids"));

        return Ok(Success(result));
    }
}
