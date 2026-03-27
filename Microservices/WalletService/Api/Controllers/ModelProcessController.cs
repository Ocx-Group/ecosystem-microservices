using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ModelProcessController : BaseController
{
    private readonly IMediator _mediator;
    public ModelProcessController(IMediator mediator) => _mediator = mediator;

    // TODO: Implement when Application layer commands/queries are created

    [HttpPost("execute_process_first")]
    public Task<IActionResult> ExecuteFirstProcess()
        => throw new NotImplementedException();

    [HttpPost("execute_process_second")]
    public Task<IActionResult> ExecuteSecondProcess()
        => throw new NotImplementedException();

    [HttpPost("eco_pool_configuration")]
    public Task<IActionResult> EcoPoolConfiguration([FromBody] object request)
        => throw new NotImplementedException();

    [HttpGet]
    public Task<IActionResult> GetEcoPoolConfiguration()
        => throw new NotImplementedException();

    [HttpGet("GetProgressPercentage/{configurationId:int}")]
    public Task<IActionResult> GetProgressPercentage([FromRoute] int configurationId)
        => throw new NotImplementedException();
}
