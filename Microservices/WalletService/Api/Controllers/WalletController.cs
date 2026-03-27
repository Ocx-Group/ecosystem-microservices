using Asp.Versioning;
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

    // TODO: Implement when Application layer commands/queries are created

    [HttpGet("GetAllWallets")]
    public Task<IActionResult> GetAllWallets()
        => throw new NotImplementedException();

    [HttpGet("GetWalletsRequest")]
    public Task<IActionResult> GetWalletsRequest([FromQuery] int userId)
        => throw new NotImplementedException();

    [HttpGet("{id:int}")]
    public Task<IActionResult> GetWalletById(int id)
        => throw new NotImplementedException();

    [HttpGet("GetWalletByAffiliateId/{id:int}")]
    public Task<IActionResult> GetWalletByAffiliateId(int id)
        => throw new NotImplementedException();

    [HttpGet("GetWalletByUserId/{id:int}")]
    public Task<IActionResult> GetWalletByUserId(int id)
        => throw new NotImplementedException();

    [HttpDelete("{id:int}")]
    public Task<IActionResult> DeleteWalletAsync([FromRoute] int id)
        => throw new NotImplementedException();

    [HttpGet("GetBalanceInformationByAffiliateId/{id:int}")]
    public Task<IActionResult> GetBalanceInformationByAffiliateId(int id)
        => throw new NotImplementedException();

    [HttpGet("GetBalanceInformationAdmin")]
    public Task<IActionResult> GetBalanceInformationAdmin()
        => throw new NotImplementedException();

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
    public Task<IActionResult> TransferBalanceForNewAffiliates([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPut("transferBalanceForNewAffiliatesMobile")]
    public Task<IActionResult> TransferBalanceForNewAffiliatesMobile([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("transferBalance")]
    public Task<IActionResult> TransferBalance([FromBody] string encrypted)
        => throw new NotImplementedException();

    [HttpPost("rejectOrCancelRevertDebitTransaction")]
    public Task<IActionResult> RejectOrCancelRevertDebitTransaction([FromQuery] int option, [FromBody] int id)
        => throw new NotImplementedException();

    [HttpGet("getPurchasesMadeInMyNetwork/{id:int}")]
    public Task<IActionResult> GetPurchasesMadeInMyNetwork([FromRoute] int id)
        => throw new NotImplementedException();

    [HttpGet("GetAllAffiliatesWithPositiveBalance")]
    public Task<IActionResult> GetAllAffiliatesWithPositiveBalance()
        => throw new NotImplementedException();

    [HttpPost("createCreditAdmin")]
    public Task<IActionResult> CreateCreditAdmin([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("payWithMyBalanceCourses")]
    public Task<IActionResult> PayWithMyBalanceCourses([FromBody] object request)
        => throw new NotImplementedException();

    [HttpPost("RemoveKeys")]
    public Task<IActionResult> RemoveKeys([FromBody] object request)
        => throw new NotImplementedException();
}
