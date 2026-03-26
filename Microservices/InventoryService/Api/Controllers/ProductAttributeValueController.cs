using Asp.Versioning;
using Ecosystem.InventoryService.Application.Commands.ProductAttributeValue;
using Ecosystem.InventoryService.Application.Queries.ProductAttributeValue;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.InventoryService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductAttributeValueController : BaseController
{
    private readonly IMediator _mediator;

    public ProductAttributeValueController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllProductAttributeValuesQuery(), ct);
        return Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductAttributeValueCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is null ? Ok(Fail("The product attribute value wasn't created")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteProductAttributeValueCommand(id), ct);
        return result is null ? Ok(Fail("The product attribute value wasn't deleted")) : Ok(Success(result));
    }
}
