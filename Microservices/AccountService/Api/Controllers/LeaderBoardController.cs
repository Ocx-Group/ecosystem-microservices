using Asp.Versioning;
using Ecosystem.AccountService.Application.Commands.LeaderBoard;
using Ecosystem.AccountService.Application.Queries.LeaderBoard;
using Ecosystem.AccountService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LeaderBoardController : BaseController
{
    private readonly IMediator _mediator;

    public LeaderBoardController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("model4/getTree")]
    public async Task<IActionResult> GetTreeModel4([FromQuery] int? id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTreeModel4Query(id), ct);
        return result is not null
            ? Ok(Success(result))
            : Ok(Fail("Tree not found"));
    }

    [HttpPost("model4/getResultTree")]
    public async Task<IActionResult> GetResultTreeModel4([FromBody] Dictionary<int, decimal> volumeData, CancellationToken ct)
    {
        var result = await _mediator.Send(new CalculateResultLeaderBoardCommand(volumeData), ct);
        return Ok(Success(result));
    }

    [HttpPost("model4/deleteTree")]
    public async Task<IActionResult> DeleteTreeModel4(CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteLeaderBoardModel4Command(), ct);
        return Ok(Success(result));
    }

    [HttpGet("model5/getTree")]
    public async Task<IActionResult> GetTreeModel5([FromQuery] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTreeModel5Query(id), ct);
        return result is not null
            ? Ok(Success(result))
            : Ok(Fail("Tree not found"));
    }

    [HttpPost("model5/addTree")]
    public async Task<IActionResult> AddTreeModel5([FromBody] List<LeaderBoardModel5> leaderBoard, CancellationToken ct)
    {
        var result = await _mediator.Send(new AddLeaderBoardModel5Command(leaderBoard), ct);
        return Ok(Success(result));
    }

    [HttpPost("model5/deleteTree")]
    public async Task<IActionResult> DeleteTreeModel5(CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteLeaderBoardModel5Command(), ct);
        return Ok(Success(result));
    }

    [HttpGet("model6/getTree")]
    public async Task<IActionResult> GetTreeModel6([FromQuery] int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTreeModel6Query(id), ct);
        return Ok(Success(result));
    }

    [HttpPost("model6/addTree")]
    public async Task<IActionResult> AddTreeModel6([FromBody] List<LeaderBoardModel6> leaderBoard, CancellationToken ct)
    {
        var result = await _mediator.Send(new AddLeaderBoardModel6Command(leaderBoard), ct);
        return Ok(Success(result));
    }

    [HttpPost("model6/deleteTree")]
    public async Task<IActionResult> DeleteTreeModel6(CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteLeaderBoardModel6Command(), ct);
        return Ok(Success(result));
    }
}
