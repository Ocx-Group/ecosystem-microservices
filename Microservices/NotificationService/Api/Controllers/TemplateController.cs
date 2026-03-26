using Asp.Versioning;
using Ecosystem.NotificationService.Application.Commands.Template;
using Ecosystem.NotificationService.Application.Queries.Template;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.NotificationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TemplateController : BaseController
{
    private readonly IMediator _mediator;

    public TemplateController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] long? brandId)
    {
        var result = await _mediator.Send(new GetAllTemplatesQuery(brandId));
        return Ok(Success(result));
    }

    [HttpGet("{templateKey}/brand/{brandId:long}")]
    public async Task<IActionResult> GetByKey(string templateKey, long brandId)
    {
        var result = await _mediator.Send(new GetTemplateByKeyQuery(templateKey, brandId));
        return result is null
            ? NotFound(Fail($"Template '{templateKey}' not found for brand {brandId}"))
            : Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTemplateCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetByKey),
            new { templateKey = result.TemplateKey, brandId = result.BrandId },
            Success(result));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateTemplateCommand command)
    {
        var updatedCommand = command with { Id = id };
        var result = await _mediator.Send(updatedCommand);
        return Ok(Success(result));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteTemplateCommand(id));
        return result ? Ok(Success("Template deleted")) : NotFound(Fail("Template not found"));
    }
}
