using Asp.Versioning;
using Ecosystem.NotificationService.Application.Commands.Brand;
using Ecosystem.NotificationService.Application.Queries.Brand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.NotificationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/email-sender-config")]
public class BrandConfigurationController : BaseController
{
    private readonly IMediator _mediator;

    public BrandConfigurationController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllBrandConfigurationsQuery());
        return Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBrandConfigurationCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), Success(result));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateBrandConfigurationCommand command)
    {
        var updatedCommand = command with { Id = id };
        var result = await _mediator.Send(updatedCommand);
        return Ok(Success(result));
    }
}
