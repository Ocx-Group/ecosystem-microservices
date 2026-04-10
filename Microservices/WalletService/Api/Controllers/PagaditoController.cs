using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.Pagadito;
using Ecosystem.WalletService.Domain.Requests.PagaditoRequest;
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

    [HttpPost("create_transaction")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreatePagaditoTransactionRequest request)
    {
        var command = new CreatePagaditoTransactionCommand
        {
            Amount = request.Amount,
            AffiliateId = request.AffiliateId,
            Details = request.Details,
            CustomParams = request.CustomParams
        };

        var result = await _mediator.Send(command);
        return result is null ? Ok(Fail("Error")) : Ok(Success(result));
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> HandleWebhook()
    {
        var headers = Request.Headers.ToDictionary(
            header => header.Key,
            header => header.Value.ToString(),
            StringComparer.OrdinalIgnoreCase);

        Request.EnableBuffering();
        string requestBody;
        using (var reader = new StreamReader(Request.Body, leaveOpen: true))
        {
            requestBody = await reader.ReadToEndAsync();
        }
        Request.Body.Position = 0;

        var isSignatureValid = await _mediator.Send(new ProcessPagaditoWebhookCommand(headers, requestBody));

        return isSignatureValid
            ? Ok()
            : BadRequest("The request is not valid or the purchase could not be processed.");
    }
}
