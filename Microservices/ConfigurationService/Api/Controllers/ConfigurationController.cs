using Asp.Versioning;
using Ecosystem.ConfigurationService.Application.Commands.Configuration;
using Ecosystem.ConfigurationService.Application.Queries.Configuration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.ConfigurationService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ConfigurationController : BaseController
{
    private readonly IMediator _mediator;
    public ConfigurationController(IMediator mediator) => _mediator = mediator;

    [HttpPost("matrix_configuration")]
    public async Task<IActionResult> MatrixConfiguration([FromBody] SetMatrixConfigurationCommand command, CancellationToken ct)
        => Ok(Success(await _mediator.Send(command, ct)));

    [HttpGet("get_matrix_configuration")]
    public async Task<IActionResult> GetMatrixConfiguration(CancellationToken ct)
        => Ok(Success(await _mediator.Send(new GetMatrixConfigurationQuery(), ct)));

    [HttpPost("product_configuration")]
    public async Task<IActionResult> ProductConfiguration([FromBody] SetProductConfigurationCommand command, CancellationToken ct)
        => Ok(Success(await _mediator.Send(command, ct)));

    [HttpGet("get_product_configuration")]
    public async Task<IActionResult> GetProductConfiguration(CancellationToken ct)
        => Ok(Success(await _mediator.Send(new GetProductConfigurationQuery(), ct)));

    [HttpPost("compensation_plans_configuration")]
    public async Task<IActionResult> CompensationPlansConfiguration([FromBody] SetCompensationPlansConfigurationCommand command, CancellationToken ct)
        => Ok(Success(await _mediator.Send(command, ct)));

    [HttpGet("get_compensation_plans_configuration")]
    public async Task<IActionResult> GetCompensationPlansConfiguration(CancellationToken ct)
        => Ok(Success(await _mediator.Send(new GetCompensationPlansConfigurationQuery(), ct)));

    [HttpPost("withdrawals_wallet_configuration")]
    public async Task<IActionResult> WithdrawalsWalletConfiguration([FromBody] SetWithdrawalsWalletConfigurationCommand command, CancellationToken ct)
        => Ok(Success(await _mediator.Send(command, ct)));

    [HttpGet("get_withdrawals_wallet_configuration")]
    public async Task<IActionResult> GetWithdrawalsWalletConfiguration(CancellationToken ct)
        => Ok(Success(await _mediator.Send(new GetWithdrawalsWalletConfigurationQuery(), ct)));

    [HttpPost("additional_parameters_wallet_configuration")]
    public async Task<IActionResult> AdditionalParametersWalletConfiguration([FromBody] SetAdditionalParametersWalletConfigurationCommand command, CancellationToken ct)
        => Ok(Success(await _mediator.Send(command, ct)));

    [HttpGet("get_additional_parameters_wallet_configuration")]
    public async Task<IActionResult> GetAdditionalParametersWalletConfiguration(CancellationToken ct)
        => Ok(Success(await _mediator.Send(new GetAdditionalParametersWalletConfigurationQuery(), ct)));

    [HttpGet("get_points_configuration")]
    public async Task<IActionResult> GetPointsConfiguration(CancellationToken ct)
        => Ok(await _mediator.Send(new GetPointsConfigurationQuery(), ct));

    [HttpGet("is_under_maintenance")]
    public async Task<IActionResult> IsUnderMaintenance(CancellationToken ct)
        => Ok(Success(await _mediator.Send(new IsUnderMaintenanceQuery(), ct)));

    [HttpPost("set_general_configuration")]
    public async Task<IActionResult> SetGeneralConfiguration([FromBody] SetGeneralConfigurationCommand command, CancellationToken ct)
        => Ok(Success(await _mediator.Send(command, ct)));

    [HttpGet("get_general_configuration")]
    public async Task<IActionResult> GetGeneralConfiguration(CancellationToken ct)
        => Ok(Success(await _mediator.Send(new GetGeneralConfigurationQuery(), ct)));
}
