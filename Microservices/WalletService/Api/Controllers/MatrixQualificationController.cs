using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.MatrixQualification;
using Ecosystem.WalletService.Application.Queries.MatrixQualification;
using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Requests.MatrixQualification;
using Ecosystem.WalletService.Domain.Requests.MatrixRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MatrixQualificationController : BaseController
{
    private const string PositionNotFoundMessage = "Error, The position not found.";
    private readonly IMediator _mediator;
    public MatrixQualificationController(IMediator mediator) => _mediator = mediator;

    [HttpPut]
    public async Task<IActionResult> UpdateQualification([FromBody] MatrixQualificationRequest request)
    {
        var command = new UpdateMatrixQualificationCommand
        {
            QualificationId = request.QualificationId,
            UserId = request.UserId,
            MatrixType = request.MatrixType,
            TotalEarnings = request.TotalEarnings,
            WithdrawnAmount = request.WithdrawnAmount,
            AvailableBalance = request.AvailableBalance,
            IsQualified = request.IsQualified
        };

        var result = await _mediator.Send(command);
        return result is null ? Ok(Fail("The qualification wasn't updated")) : Ok(Success(result));
    }

    [HttpGet("get_by_user_and_matrix_type")]
    public async Task<IActionResult> GetByUserAndMatrixType([FromQuery] MatrixRequest request)
    {
        var result = await _mediator.Send(new GetMatrixQualificationByUserQuery(request.UserId, request.MatrixType));
        return result is null ? Ok(Fail(PositionNotFoundMessage)) : Ok(Success(result));
    }

    [HttpGet("process_qualification")]
    public async Task<IActionResult> ProcessQualification([FromQuery] int userId)
    {
        var result = await _mediator.Send(new ProcessQualificationQuery(userId));
        return !result.AnyQualified ? Ok(Fail(PositionNotFoundMessage)) : Ok(Success(result));
    }

    [HttpPost("process_qualification_admin")]
    public async Task<IActionResult> ProcessQualificationAdmin([FromBody] MatrixRequest request)
    {
        var result = await _mediator.Send(new ProcessQualificationAdminCommand(request.UserId, request.MatrixType));
        return !result ? Ok(Fail("Error, The user is active in the matrix.")) : Ok(Success(result));
    }

    [HttpPost("process_direct_payment_matrix_activation_async")]
    public async Task<IActionResult> ProcessDirectPaymentMatrixActivationAsync([FromBody] MatrixRequest request)
    {
        var command = new ProcessDirectPaymentMatrixCommand
        {
            UserId = request.UserId,
            MatrixType = request.MatrixType,
            RecipientId = request.RecipientId,
            Cycle = request.Cycle
        };

        var result = await _mediator.Send(command);
        return !result ? Ok(Fail(PositionNotFoundMessage)) : Ok(Success(result));
    }

    [HttpPost("check_qualification")]
    public async Task<IActionResult> CheckQualification([FromBody] MatrixRequest request)
    {
        var result = await _mediator.Send(new CheckQualificationCommand(request.UserId, request.MatrixType));
        return !result ? Ok(Fail(PositionNotFoundMessage)) : Ok(Success(result));
    }

    [HttpPost("all_inconsistent_records")]
    public async Task<IActionResult> FixInconsistentQualificationRecordsAsync()
    {
        var result = await _mediator.Send(new GetAllInconsistentRecordsCommand());
        return Ok(Success(result));
    }

    [HttpPost("process_all_qualifications")]
    public async Task<IActionResult> ProcessAllQualifications([FromBody] int[]? userIds = null)
    {
        var result = await _mediator.Send(new ProcessAllQualificationsCommand(userIds));
        return Ok(Success(result));
    }

    [HttpPost("coinpayments_matrix_activation_confirmation")]
    public async Task<IActionResult> CoinPaymentsMatrixActivationConfirmation([FromForm] IpnRequest request)
    {
        var headers = Request.Headers.ToDictionary(
            header => header.Key,
            header => header.Value.ToString(),
            StringComparer.OrdinalIgnoreCase);

        var result = await _mediator.Send(new CoinPaymentsMatrixActivationCommand(request, headers));
        return !result ? BadRequest() : Ok("IPN OK");
    }

    [HttpGet("has-reached-withdrawal-limit")]
    public async Task<IActionResult> HasReachedWithdrawalLimit([FromQuery] int userId)
    {
        var result = await _mediator.Send(new HasReachedWithdrawalLimitQuery(userId));
        return Ok(Success(result));
    }
}
