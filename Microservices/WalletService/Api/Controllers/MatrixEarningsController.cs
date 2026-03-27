using Asp.Versioning;
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

    // TODO: Implement when Application layer commands/queries are created

    [HttpPost]
    public Task<IActionResult> CreateAsync([FromBody] object request)
        => throw new NotImplementedException();
}
