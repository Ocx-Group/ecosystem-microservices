using Asp.Versioning;
using Ecosystem.InventoryService.Application.Commands.ProductInventory;
using Ecosystem.InventoryService.Application.Queries.ProductInventory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.InventoryService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductInventoryController : BaseController
{
    private readonly IMediator _mediator;

    public ProductInventoryController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllProductInventoriesQuery(), ct);
        return Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductInventoryCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is null ? Ok(Fail("The product inventory wasn't created")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteProductInventoryCommand(id), ct);
        return result is null ? Ok(Fail("The product inventory wasn't deleted")) : Ok(Success(result));
    }
}
