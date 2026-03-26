using Asp.Versioning;
using Ecosystem.ConfigurationService.Application.Queries.MatrixConfiguration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.ConfigurationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MatrixConfigurationController : BaseController
{
    private readonly IMediator _mediator;
    public MatrixConfigurationController(IMediator mediator) => _mediator = mediator;

    [HttpGet("get_matrix_configuration")]
    public async Task<IActionResult> GetMatrixConfigurationByType([FromQuery] int matrixType, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMatrixConfigurationByTypeQuery(matrixType), ct);
        return result is null ? BadRequest(Fail("The Matrix Configuration wasn't found")) : Ok(Success(result));
    }

    [HttpGet("get_all_matrix_configurations")]
    public async Task<IActionResult> GetAllMatrixConfigurations(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllMatrixConfigurationsQuery(), ct);
        return result is null ? BadRequest(Fail("The Matrix Configuration wasn't found")) : Ok(Success(result));
    }
}
