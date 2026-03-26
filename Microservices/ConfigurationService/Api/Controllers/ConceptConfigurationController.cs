using Asp.Versioning;
using Ecosystem.ConfigurationService.Application.Commands.ConceptConfiguration;
using Ecosystem.ConfigurationService.Application.Queries.ConceptConfiguration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.ConfigurationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ConceptConfigurationController : BaseController
{
    private readonly IMediator _mediator;
    public ConceptConfigurationController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateConceptConfiguration([FromBody] CreateConceptConfigurationCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpGet("get_all_concept_configuration")]
    public async Task<IActionResult> GetConceptsConfigurationByConceptId(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllConceptConfigurationsByConceptIdQuery(id), ct);
        return Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateConceptConfiguration([FromRoute] int id, [FromBody] UpdateConceptConfigurationCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? BadRequest(Fail("The level wasn't updated")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteConceptConfiguration([FromRoute] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteConceptConfigurationCommand(id), ct);
        return result is null ? BadRequest(Fail("The level wasn't updated")) : Ok(Success(result));
    }
}
