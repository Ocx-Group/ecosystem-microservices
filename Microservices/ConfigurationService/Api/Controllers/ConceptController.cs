using Asp.Versioning;
using Ecosystem.ConfigurationService.Application.Commands.Concept;
using Ecosystem.ConfigurationService.Application.Queries.Concept;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.ConfigurationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ConceptController : BaseController
{
    private readonly IMediator _mediator;
    public ConceptController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateConcept([FromBody] CreateConceptCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllConcept(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllConceptsQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_concept")]
    public async Task<IActionResult> GetConceptById([FromQuery] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetConceptByIdQuery(id), ct);
        return Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateConcept([FromRoute] int id, [FromBody] UpdateConceptCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? BadRequest(Fail("The concept wasn't updated")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteConcept([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteConceptCommand(id), ct);
        return result is null ? BadRequest(Fail("The concept wasn't deleted")) : Ok(Success(result));
    }
}
