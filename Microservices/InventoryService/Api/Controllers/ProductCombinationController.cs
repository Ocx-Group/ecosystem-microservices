using Asp.Versioning;
using Ecosystem.InventoryService.Application.Commands.ProductCombination;
using Ecosystem.InventoryService.Application.Queries.ProductCombination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.InventoryService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductCombinationController : BaseController
{
    private readonly IMediator _mediator;

    public ProductCombinationController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllProductCombinationsQuery(), ct);
        return Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCombinationCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is null ? Ok(Fail("The product combination wasn't created")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteProductCombinationCommand(id), ct);
        return result is null ? Ok(Fail("The product combination wasn't deleted")) : Ok(Success(result));
    }
}
