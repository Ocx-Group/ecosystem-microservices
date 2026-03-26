using Asp.Versioning;
using Ecosystem.AccountService.Application.Commands.User;
using Ecosystem.AccountService.Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecosystem.AccountService.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : BaseController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("get_all")]
    public async Task<IActionResult> GetUsers(CancellationToken ct)
    {
        var users = await _mediator.Send(new GetUsersQuery(), ct);
        return Ok(Success(users));
    }

    [HttpGet("get_user")]
    public async Task<IActionResult> GetUserById([FromQuery] int id, CancellationToken ct)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id), ct);
        return user is not null
            ? Ok(Success(user))
            : Ok(Fail("User not found"));
    }

    [HttpGet("get_users_rol_id")]
    public async Task<IActionResult> GetUsersByRolId([FromQuery] int id, CancellationToken ct)
    {
        var users = await _mediator.Send(new GetUsersByRolIdQuery(id), ct);
        return Ok(Success(users));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var user = await _mediator.Send(updated, ct);
        return user is not null
            ? Ok(Success(user))
            : Ok(Fail("User not found or invalid data"));
    }

    [HttpPut("update_password_user/{id}")]
    public async Task<IActionResult> UpdatePasswordUser(int id, [FromBody] UpdatePasswordCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var user = await _mediator.Send(updated, ct);
        return user is not null
            ? Ok(Success(user))
            : Ok(Fail("Invalid password or user not found"));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken ct)
    {
        var user = await _mediator.Send(command, ct);
        return user is not null
            ? Ok(Success(user))
            : Ok(Fail("Error creating user"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id), ct);
        return result
            ? Ok(Success("User deleted"))
            : Ok(Fail("User not found"));
    }

    [HttpPut("update_image_profile_url/{id}")]
    public async Task<IActionResult> UpdateImageProfile(int id, [FromBody] UpdateImageProfileCommand command, CancellationToken ct)
    {
        var updated = command with { Id = id };
        var user = await _mediator.Send(updated, ct);
        return user is not null
            ? Ok(Success(user))
            : Ok(Fail("User not found"));
    }
}
