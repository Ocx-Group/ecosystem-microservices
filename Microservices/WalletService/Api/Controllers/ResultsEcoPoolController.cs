using Asp.Versioning;
using Ecosystem.WalletService.Application.Queries.ResultsEcoPool;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/ResultsEcoPool")]
public class ResultsEcoPoolController : BaseController
{
    private readonly IMediator _mediator;
    public ResultsEcoPoolController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAllResultsEcoPool")]
    public async Task<IActionResult> GetAllResultsEcoPool(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllResultsEcoPoolQuery(), ct);
        return Ok(Success(result));
    }
}
