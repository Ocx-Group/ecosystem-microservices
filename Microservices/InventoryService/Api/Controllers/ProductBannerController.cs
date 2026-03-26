using Asp.Versioning;
using Ecosystem.InventoryService.Application.Commands.ProductBanner;
using Ecosystem.InventoryService.Application.Queries.ProductBanner;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.InventoryService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductBannerController : BaseController
{
    private readonly IMediator _mediator;

    public ProductBannerController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllProductBannersQuery(), ct);
        return Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductBannerCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is null ? Ok(Fail("The product banner wasn't created")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteProductBannerCommand(id), ct);
        return result is null ? Ok(Fail("The product banner wasn't deleted")) : Ok(Success(result));
    }
}
