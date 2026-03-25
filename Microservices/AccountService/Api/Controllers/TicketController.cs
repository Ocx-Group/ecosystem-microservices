using Asp.Versioning;
using Ecosystem.AccountService.Application.Commands.Ticket;
using Ecosystem.AccountService.Application.Queries.Ticket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TicketController : BaseController
{
    private readonly IMediator _mediator;

    public TicketController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateTicketAsync(
        [FromBody] CreateTicketCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is null ? Ok(Fail("Ticket could not be created.")) : Ok(Success(result));
    }

    [HttpGet("getAllTickets/{affiliateId:int}")]
    public async Task<IActionResult> GetAllTicketsByAffiliateId(
        [FromRoute] int affiliateId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllTicketsByAffiliateIdQuery(affiliateId), ct);
        return Ok(Success(result));
    }
}
