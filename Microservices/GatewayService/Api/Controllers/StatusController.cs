using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.GatewayService.Api.Controllers;

[ApiController]
[Route("api/v1/gateway")]
[AllowAnonymous]
public class StatusController : ControllerBase
{
    private static readonly DateTime StartedAt = DateTime.UtcNow;

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            service = "Ecosystem.GatewayService",
            version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0",
            status = "healthy",
            startedAt = StartedAt,
            uptime = DateTime.UtcNow - StartedAt
        });
    }
}

