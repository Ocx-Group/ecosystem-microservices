using Asp.Versioning;
using Ecosystem.InventoryService.Application.Commands.Product;
using Ecosystem.InventoryService.Application.Queries.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.InventoryService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductController : BaseController
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
        => _mediator = mediator;

    public record GetProductsByIdsRequest(long[] ProductIds);
    public record GetProductByIdRequest(int Id);

    [HttpGet]
    public async Task<IActionResult> GetAllEcoPools(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllEcoPoolsQuery(), ct);
        return Ok(Success(result));
    }

    [HttpPost("get_products_by_ids")]
    public async Task<IActionResult> GetProductsByIds([FromBody] GetProductsByIdsRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllProductsByIdsQuery(request.ProductIds), ct);
        return Ok(Success(result));
    }

    [HttpPost("get_product_by_id")]
    public async Task<IActionResult> GetProductById([FromBody] GetProductByIdRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(request.Id), ct);
        return Ok(Success(result));
    }

    [HttpGet("getAllProductsAdmin")]
    public async Task<IActionResult> GetAllProductsAdmin(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllProductsAdminQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("membership")]
    public async Task<IActionResult> GetAllMembership(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllMembershipQuery(), ct);
        return Ok(Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is null ? Ok(Fail("The product wasn't created")) : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id), ct);
        return result is null ? Ok(Fail("The product wasn't deleted")) : Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The product wasn't updated")) : Ok(Success(result));
    }

    [HttpGet("get_all_recurring")]
    public async Task<IActionResult> GetAllRecurring(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllRecurringQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_all_services")]
    public async Task<IActionResult> GetAllServices(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllServicesQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_all_funding_accounts")]
    public async Task<IActionResult> GetAllFundingAccounts(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllFundingAccountsQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_all_trading_academy")]
    public async Task<IActionResult> GetAllTradingAcademy(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllTradingAcademyQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_all_savings_plans")]
    public async Task<IActionResult> GetAllSavingsPlans(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllSavingsPlansQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_all_savings_plans_one_b")]
    public async Task<IActionResult> GetAllSavingsPlansOneB(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllSavingsPlansOneBQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_all_alternative_health")]
    public async Task<IActionResult> GetAllAlternativeHealth(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllAlternativeHealthQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_all_alternative_health_for_europe")]
    public async Task<IActionResult> GetAllAlternativeHealthForEurope(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllAlternativeHealthForEuropeQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_all_recycoin")]
    public async Task<IActionResult> GetAllRecyCoin(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllRecyCoinQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("get_all_with_payment_group/{paymentGroupId:int}")]
    public async Task<IActionResult> GetAllWithPaymentGroup(int paymentGroupId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllWithPaymentGroupQuery(paymentGroupId), ct);
        return Ok(Success(result));
    }
}
