using Asp.Versioning;
using Ecosystem.InventoryService.Application.Commands.ProductCategory;
using Ecosystem.InventoryService.Application.Queries.ProductCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.InventoryService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductCategoryController : BaseController
{
    private readonly IMediator _mediator;

    public ProductCategoryController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllProductCategoriesQuery(), ct);
        return Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCategoryCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is null ? Ok(Fail("The product category wasn't created")) : Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCategoryCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The product category wasn't updated")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteProductCategoryCommand(id), ct);
        return result is null ? Ok(Fail("The product category wasn't deleted")) : Ok(Success(result));
    }
}
