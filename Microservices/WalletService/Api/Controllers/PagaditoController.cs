using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/Pagadito")]
public class PagaditoController : BaseController
{
    private readonly IMediator _mediator;
    public PagaditoController(IMediator mediator) => _mediator = mediator;

    // TODO: Implement when Application layer commands/queries are created

    [HttpPost("create_transaction")]
    public Task<IActionResult> CreateTransaction([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("webhook")]
    public Task<IActionResult> HandleWebhook()
        => throw new NotImplementedException();
}
