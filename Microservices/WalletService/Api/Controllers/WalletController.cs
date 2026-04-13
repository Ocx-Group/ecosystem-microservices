using Asp.Versioning;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.WalletService.Application.Queries.Wallet;
using Ecosystem.WalletService.Domain.Requests.TransferBalanceRequest;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.WalletService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/Wallet")]
public class WalletController : BaseController
{
    private readonly IMediator _mediator;
    public WalletController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAllWallets")]
    public async Task<IActionResult> GetAllWallets()
    {
        var result = await _mediator.Send(new GetAllWalletsQuery());
        return Ok(Success(result));
    }

    [HttpGet("GetWalletsRequest")]
    public async Task<IActionResult> GetWalletsRequest([FromQuery] int userId)
    {
        var result = await _mediator.Send(new GetWalletsRequestQuery(userId));
        return Ok(Success(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWalletById(int id)
    {
        var result = await _mediator.Send(new GetWalletByIdQuery(id));
        return result is null ? Ok(Fail("The wallet wasn't found")) : Ok(Success(result));
    }

    [HttpGet("GetWalletByAffiliateId/{id:int}")]
    public async Task<IActionResult> GetWalletByAffiliateId(int id)
    {
        var result = await _mediator.Send(new GetWalletByAffiliateIdQuery(id));
        return !result.Any()
            ? Ok(Fail("The wallet wasn't found"))
            : Ok(Success(result));
    }

    [HttpGet("GetWalletByUserId/{id:int}")]
    public async Task<IActionResult> GetWalletByUserId(int id)
    {
        var result = await _mediator.Send(new GetWalletByUserIdQuery(id));
        return !result.Any()
            ? Ok(Fail("The wallet wasn't found"))
            : Ok(Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWalletAsync([FromRoute] int id)
    {
        var result = await _mediator.Send(new DeleteWalletCommand(id));
        return result is null ? BadRequest("The wallet wasn't delete") : Ok(Success(result));
    }

    [HttpGet("GetBalanceInformationByAffiliateId/{id:int}")]
    public async Task<IActionResult> GetBalanceInformationByAffiliateId(int id)
    {
        var result = await _mediator.Send(new GetBalanceInformationQuery(id));
        return Ok(Success(result));
    }

    [HttpGet("GetBalanceInformationAdmin")]
    public async Task<IActionResult> GetBalanceInformationAdmin()
    {
        var result = await _mediator.Send(new GetBalanceInformationAdminQuery());
        return Ok(Success(result));
    }

    [HttpPost("payWithMyBalance")]
    public Task<IActionResult> PayWithMyBalance([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("payWithMyBalanceForOthers")]
    public Task<IActionResult> PayWithMyBalanceForOthers([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("payWithMyBalanceModel2")]
    public Task<IActionResult> PayWithMyBalanceModel2([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("payMembershipWithMyBalance")]
    public Task<IActionResult> PayMembershipWithMyBalance([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("payWithMyBalanceAdmin")]
    public Task<IActionResult> PayWithMyBalanceAdmin([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPut("payWithMyBalance")]
    public Task<IActionResult> PayWithMyBalanceMobile([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("transferBalanceForNewAffiliates")]
    public async Task<IActionResult> TransferBalanceForNewAffiliates([FromBody] TransferBalanceRequest request)
    {
        var response = await _mediator.Send(new TransferBalanceForNewAffiliateCommand(request));
        return Ok(response);
    }
    
    [HttpPost("transferBalance")]
    public async Task<IActionResult> TransferBalance([FromBody] string encrypted)
    {
        var response = await _mediator.Send(new TransferBalanceCommand(encrypted));
        return Ok(response);
    }

    [HttpPost("rejectOrCancelRevertDebitTransaction")]
    public Task<IActionResult> RejectOrCancelRevertDebitTransaction([FromQuery] int option, [FromBody] int id)
        => throw new NotImplementedException();

    [HttpGet("getPurchasesMadeInMyNetwork/{id:int}")]
    public async Task<IActionResult> GetPurchasesMadeInMyNetwork([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetPurchasesMadeInMyNetworkQuery(id));

        if (result == null)
            return Ok(Fail("No purchases found on the network"));

        var response = new
        {
            result.CurrentYearPurchases,
            result.PreviousYearPurchases
        };

        return Ok(Success(response));
    }

    [HttpGet("GetAllAffiliatesWithPositiveBalance")]
    public async Task<IActionResult> GetAllAffiliatesWithPositiveBalance()
    {
        var result = await _mediator.Send(new GetAllAffiliatesWithPositiveBalanceQuery());
        return Ok(Success(result));
    }

    [HttpPost("createCreditAdmin")]
    public async Task<IActionResult> CreateCreditAdmin([FromBody] CreditTransactionAdminRequest request)
    {
        var response = await _mediator.Send(new CreateCreditAdminCommand(request));
        return !response ? Ok(Fail("The payment could not be processed")) : Ok(Success(response));
    }

    [HttpPost("payWithMyBalanceCourses")]
    public Task<IActionResult> PayWithMyBalanceCourses([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("RemoveKeys")]
    public async Task<IActionResult> RemoveKeys([FromBody] DeleteKeysRequest request)
    {
        await _mediator.Send(new RemoveKeysCommand(request));
        return Ok(Success("Ok"));
    }
}
