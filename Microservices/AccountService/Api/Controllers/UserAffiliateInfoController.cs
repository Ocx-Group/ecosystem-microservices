using Asp.Versioning;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.Queries.Affiliate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserAffiliateInfoController : BaseController
{
    private readonly IMediator _mediator;

    public UserAffiliateInfoController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateAffiliate([FromBody] CreateAffiliateCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is { Success: false } ? Ok(Fail(result.Message)) : Ok(result);
    }

    [HttpGet("get_all")]
    public async Task<IActionResult> GetUsers(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAffiliatesQuery(), ct);
        return Ok(Success(result));
    }

    [HttpPost("get_accounts_eco_pool")]
    public async Task<IActionResult> GetAccountsToEcoPool([FromBody] GetAccountsEcoPoolCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpGet("get_all_without_authorization")]
    public async Task<IActionResult> GetUsersWithoutAuthorization(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAffiliatesWithoutAuthorizationQuery(), ct);
        return Ok(Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAffiliate(int id, [FromBody] UpdateAffiliateCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The affiliate wasn't updated")) : Ok(Success(result));
    }

    [HttpPut("update_image/{id:int}")]
    public async Task<IActionResult> UpdateImage(int id, [FromBody] UpdateAffiliateImageCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The affiliate wasn't updated")) : Ok(Success(result));
    }

    [HttpPut("update_image/card_id/{id:int}")]
    public async Task<IActionResult> UpdateCardIdImage(int id, [FromBody] UpdateCardIdImageCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The affiliate wasn't updated")) : Ok(Success(result));
    }

    [HttpPut("update_activation_date/{id:int}")]
    public async Task<IActionResult> UpdateActivationDate(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateActivationDateCommand(id), ct);
        return result is null ? Ok(Fail("The affiliate wasn't updated")) : Ok(Success(result));
    }

    [HttpPut("revert_activation/{id:int}")]
    public async Task<IActionResult> RevertActivationUser(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new RevertActivationCommand(id), ct);
        return result is null ? Ok(Fail("The affiliate wasn't updated")) : Ok(Success(result));
    }

    [HttpPut("update_card_id_authorization/{id:int}")]
    public async Task<IActionResult> UpdateCardIdAuthorization(int id, [FromBody] int option, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateCardIdAuthorizationCommand(id, option), ct);
        return result is null ? Ok(Fail("The affiliate wasn't updated")) : Ok(Success(result));
    }

    [HttpGet("url_signup_generate/{userId:int}")]
    public async Task<IActionResult> UrlSignUpGenerate(int userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUrlSignUpQuery(userId), ct);
        return result is null ? Ok(Fail("The url wasn't created")) : Ok(Success(result));
    }

    [HttpPut("email_confirmation/{userName}")]
    public async Task<IActionResult> EmailConfirmation(string userName, CancellationToken ct)
    {
        await _mediator.Send(new EmailConfirmationCommand(userName), ct);
        return Ok(Success("ok"));
    }

    [HttpPost("authorization_affiliates")]
    public async Task<IActionResult> AuthorizationAffiliates([FromBody] AuthorizationAffiliatesCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(Success("ok"));
    }

    [HttpGet("get_user_username/{userName}")]
    public async Task<IActionResult> GetAffiliateByUserName(string userName, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAffiliateByUserNameQuery(userName), ct);
        return result is null ? Ok(Fail("The user wasn't find")) : Ok(Success(result));
    }

    [HttpGet("get_user_id/{id:int}")]
    public async Task<IActionResult> GetAffiliateById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAffiliateByIdQuery(id), ct);
        return result is null ? Ok(Fail("The user wasn't find")) : Ok(Success(result));
    }

    [HttpPut("update_password_user/{id:int}")]
    public async Task<IActionResult> UpdatePasswordUser(int id, [FromBody] UpdateAffiliatePasswordCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The password user wasn't updated")) : Ok(Success(result));
    }

    [HttpPut("update_pin/{id:int}")]
    public async Task<IActionResult> UpdatePinUser(int id, [FromBody] UpdatePinCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The pin user wasn't updated")) : Ok(Success(result));
    }

    [HttpPut("secret_question/{id:int}")]
    public async Task<IActionResult> UpdateSecretQuestion(int id, [FromBody] UpdateSecretQuestionCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The password user wasn't updated")) : Ok(Success(result));
    }

    [HttpPost("send_email_confirmation/{id:int}")]
    public async Task<IActionResult> SendEmailConfirmation(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new SendEmailConfirmationCommand(id), ct);
        return result is false ? Ok(Fail("The user wasn't find")) : Ok(Success(result));
    }

    [HttpPut("update_user_profile/{id:int}")]
    public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] UpdateUserProfileCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The affiliate wasn't updated")) : Ok(Success(result));
    }

    [HttpPut("update_image_id_path/{id:int}")]
    public async Task<IActionResult> UpdateImageIdPath(int id, [FromBody] UpdateImageIdPathCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var result = await _mediator.Send(updated, ct);
        return result is null ? Ok(Fail("The affiliate wasn't updated")) : Ok(Success(result));
    }

    [HttpPost("generateVerificationCode/{id:int}")]
    public async Task<IActionResult> GenerateVerificationCode(int id, [FromQuery] bool checkDate, CancellationToken ct)
    {
        var result = await _mediator.Send(new GenerateVerificationCodeCommand(id, checkDate), ct);
        return result is false ? Ok(Fail("Failed to generate verification code")) : Ok(Success(result));
    }

    [HttpPost("validationCode")]
    public async Task<IActionResult> ValidationCode([FromBody] ValidationCodeCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result is false ? Ok(Fail("Failed to verificate code")) : Ok(Success(result));
    }

    [HttpGet("getPersonalNetwork/{id:int}")]
    public async Task<IActionResult> GetPersonalNetwork(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPersonalNetworkQuery(id), ct);
        return Ok(Success(result));
    }

    [HttpGet("getTotalActiveMembers")]
    public async Task<IActionResult> GetTotalActiveMembers(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTotalActiveMembersQuery(), ct);
        return Ok(Success(result));
    }

    [HttpPost("sendEmailToChangePassword")]
    public async Task<IActionResult> SendEmailToChangePasswordByEmail([FromBody] string email, CancellationToken ct)
    {
        var result = await _mediator.Send(new SendEmailToChangePasswordCommand(email), ct);
        return result is false ? Ok(Fail("The email wasn't find")) : Ok(Success(result));
    }

    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpGet("getAffiliateByVerificationCode/{code}")]
    public async Task<IActionResult> GetAffiliateByVerificationCode(string code, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAffiliateByVerificationCodeQuery(code), ct);
        return result is null ? Ok(Fail("The affiliate wasn't find")) : Ok(Success(result));
    }

    [HttpGet("getTotalAffiliatesByCountries")]
    public async Task<IActionResult> GetTotalAffiliatesByCountries(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTotalAffiliatesByCountriesQuery(), ct);
        return Ok(Success(result));
    }

    [HttpGet("getUserByEmail/{email}")]
    public async Task<IActionResult> GetAffiliateByEmail(string email, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAffiliateByEmailQuery(email), ct);
        return result is null ? Ok(Fail("The user wasn't find")) : Ok(Success(result));
    }

    [HttpPut("update_grading/{userId:int}/{gradingId:int}")]
    public async Task<IActionResult> UpdateGrading(int userId, int gradingId, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateGradingCommand(userId, gradingId), ct);
        return result is false ? Ok(Fail("The user wasn't find")) : Ok(Success(result));
    }

    [HttpPost("resendEmailConfirmation/{affiliateId:int}")]
    public async Task<IActionResult> ResendEmailConfirmation(int affiliateId, CancellationToken ct)
    {
        var result = await _mediator.Send(new ResendEmailConfirmationCommand(affiliateId), ct);
        return !result ? Ok(Fail("The email confirmation was not sent.")) : Ok(Success(result));
    }

    [HttpGet("getNetworkDetails/{affiliateId:int}")]
    public async Task<IActionResult> NetworkDetails(int affiliateId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetNetworkDetailsQuery(affiliateId), ct);
        return Ok(Success(result));
    }

    [HttpPut("update_message_alert/{id:int}")]
    public async Task<IActionResult> UpdateMessageAlert(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateMessageAlertCommand(id), ct);
        return result is null ? Ok(Fail("The affiliate wasn't updated")) : Ok(Success(result));
    }

    [HttpPost("contact_us")]
    public async Task<IActionResult> ContactUs([FromBody] ContactUsCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(Success(result));
    }

    [HttpGet("get_last_registered_users")]
    public async Task<IActionResult> GetLastRegisteredUsers(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLastRegisteredUsersQuery(), ct);
        return Ok(Success(result));
    }
}
