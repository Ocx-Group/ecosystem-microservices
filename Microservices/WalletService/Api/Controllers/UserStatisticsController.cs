using Asp.Versioning;
using Ecosystem.WalletService.Application.Queries.UserStatistics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserStatisticsController : BaseController
{
    private readonly IMediator _mediator;
    public UserStatisticsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserStatistics(int userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUserStatisticsQuery(userId), ct);
        return Ok(Success(result));
    }
}
