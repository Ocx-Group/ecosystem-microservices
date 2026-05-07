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
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllTemplatesQuery());
        return Ok(Success(result));
    }

    [HttpGet("{templateKey}")]
    public async Task<IActionResult> GetByKey(string templateKey)
    {
        var result = await _mediator.Send(new GetTemplateByKeyQuery(templateKey));
        return result is null
            ? NotFound(Fail($"Template '{templateKey}' not found"))
            : Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTemplateCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetByKey),
            new { templateKey = result.TemplateKey },
            Success(result));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateTemplateCommand command)
    {
        var updatedCommand = command with { Id = id };
        var result = await _mediator.Send(updatedCommand);
        return Ok(Success(result));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.Send(new DeleteTemplateCommand(id));
        return result ? Ok(Success("Template deleted")) : NotFound(Fail("Template not found"));
    }
}
