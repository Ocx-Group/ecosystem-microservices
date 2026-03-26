using Asp.Versioning;
using Ecosystem.AccountService.Application.DTOs.Matrix;
using Ecosystem.AccountService.Application.Queries.Matrix;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MatrixController : BaseController
{
    private readonly IMediator _mediator;

    public MatrixController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost("have_2_children")]
    public async Task<IActionResult> WhatUsersHave2Children([FromBody] UserArrayRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new WhatUsersHave2ChildrenQuery(request.Users), ct);
        return Ok(result);
    }

    [HttpGet("uni_level")]
    public async Task<IActionResult> UniLevelFamilyTree([FromQuery] int? id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUniLevelFamilyTreeQuery(id), ct);
        return result is null ? Ok(Fail("Error")) : Ok(Success(result));
    }

    [HttpPost("get_matrix_tree")]
    public async Task<IActionResult> GetMatrixTree([FromBody] MatrixTreeRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMatrixTreeQuery(request.UserId, request.MatrixType), ct);
        return result is null ? Ok(Fail("Error")) : Ok(Success(result));
    }

    [HttpGet("is_active_in_matrix")]
    public async Task<IActionResult> GetByUserAndMatrixType(
        [FromQuery] int UserId,
        [FromQuery] int MatrixType,
        [FromQuery] int Cycle,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new IsActiveInMatrixQuery(UserId, MatrixType, Cycle), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_upline_position_async")]
    public async Task<IActionResult> GetUplinePositionAsync(
        [FromQuery] int UserId,
        [FromQuery] int MatrixType,
        [FromQuery] int Cycle,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUplinePositionsQuery(UserId, MatrixType, Cycle), ct);
        return result is null ? Ok(Fail("Error, The position not found.")) : Ok(Success(result));
    }
}

public record UserArrayRequest(long[] Users);

public record MatrixTreeRequest(int UserId, int MatrixType);
