using Asp.Versioning;
using Ecosystem.NotificationService.Application.Commands.Email;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.NotificationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class EmailController : BaseController
{
    private readonly IMediator _mediator;

    public EmailController(IMediator mediator) => _mediator = mediator;

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendEmailCommand command)
    {
        var result = await _mediator.Send(command);
        return result
            ? Ok(Success("Email sent successfully"))
            : StatusCode(500, Fail("Failed to send email"));
    }
}
