using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MatrixQualificationController : BaseController
{
    private readonly IMediator _mediator;
    public MatrixQualificationController(IMediator mediator) => _mediator = mediator;

    // TODO: Implement when Application layer commands/queries are created

    [HttpPut]
    public Task<IActionResult> UpdateQualification([FromBody] object request)
        => throw new NotImplementedException();

    [HttpGet("get_by_user_and_matrix_type")]
    public Task<IActionResult> GetByUserAndMatrixType([FromQuery] int userId, [FromQuery] int matrixType)
        => throw new NotImplementedException();

    [HttpGet("process_qualification")]
    public Task<IActionResult> ProcessQualification([FromQuery] int userId)
        => throw new NotImplementedException();

    [HttpPost("process_qualification_admin")]
    public Task<IActionResult> ProcessQualificationAdmin([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("process_direct_payment_matrix_activation_async")]
    public Task<IActionResult> ProcessDirectPaymentMatrixActivationAsync([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("check_qualification")]
    public Task<IActionResult> CheckQualification([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("all_inconsistent_records")]
    public Task<IActionResult> FixInconsistentQualificationRecordsAsync()
        => throw new NotImplementedException();

    [HttpPost("process_all_qualifications")]
    public Task<IActionResult> ProcessAllQualifications([FromBody] int[]? userIds = null)
        => throw new NotImplementedException();

    [HttpPost("coinpayments_matrix_activation_confirmation")]
    public Task<IActionResult> CoinPaymentsMatrixActivationConfirmation()
        => throw new NotImplementedException();

    [HttpGet("has-reached-withdrawal-limit")]
    public Task<IActionResult> HasReachedWithdrawalLimit([FromQuery] int userId)
        => throw new NotImplementedException();
}
