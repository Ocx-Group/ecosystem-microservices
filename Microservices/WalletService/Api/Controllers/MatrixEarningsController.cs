using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.MatrixEarnings;
using Ecosystem.WalletService.Domain.Requests.MatrixEarningRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MatrixEarningsController : BaseController
{
    private readonly IMediator _mediator;
    public MatrixEarningsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] MatrixEarningRequest request)
    {
        var command = new CreateMatrixEarningCommand
        {
            UserId = request.UserId,
            MatrixType = request.MatrixType,
            Amount = request.Amount,
            SourceUserId = request.SourceUserId,
            EarningType = request.EarningType
        };

        var result = await _mediator.Send(command);
        return Ok(Success(result));
    }
}
