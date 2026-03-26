using Asp.Versioning;
using Ecosystem.AccountService.Application.Queries.MenuConfiguration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MenuConfigurationController : BaseController
{
    private readonly IMediator _mediator;

    public MenuConfigurationController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("get_all")]
    public async Task<IActionResult> GetAllMenuConfigurations(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllMenuConfigurationsQuery(), ct);
        return Ok(Success(result));
    }
}
