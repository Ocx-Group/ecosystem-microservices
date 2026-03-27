using Asp.Versioning;
using Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;
using Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.ConfigurationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BrandConfigurationController : BaseController
{
    private readonly IMediator _mediator;
    public BrandConfigurationController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllBrandConfigurationsQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("{brandId:long}")]
    public async Task<IActionResult> GetByBrandId([FromRoute] long brandId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBrandConfigurationByBrandIdQuery(brandId), ct);
        return result is null
            ? NotFound(Fail($"Brand configuration not found for BrandId {brandId}"))
            : Ok(Success(result));
    }

    [HttpPut]
    public async Task<IActionResult> Upsert([FromBody] UpsertBrandConfigurationCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpDelete("{brandId:long}")]
    public async Task<IActionResult> Delete([FromRoute] long brandId, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteBrandConfigurationCommand(brandId), ct);
        return result is null
            ? NotFound(Fail($"Brand configuration not found for BrandId {brandId}"))
            : Ok(Success(result));
    }
}
