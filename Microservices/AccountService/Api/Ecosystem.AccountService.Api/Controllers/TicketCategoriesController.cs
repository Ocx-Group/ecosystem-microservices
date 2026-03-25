using Asp.Versioning;
using Ecosystem.AccountService.Application.Queries.Ticket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TicketCategoriesController : BaseController
{
    private readonly IMediator _mediator;

    public TicketCategoriesController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("get_all")]
    public async Task<IActionResult> GetAllTicketCategories(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllTicketCategoriesQuery(), ct);
        return Ok(Success(result));
    }
}
