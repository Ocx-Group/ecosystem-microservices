using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateAffiliatePasswordCommand : IRequest<UsersAffiliatesDto?>
{
    public int Id { get; init; }

    [JsonProperty("password")] public string? Password { get; init; }

    [JsonProperty("new_password")] public string? NewPassword { get; init; }
}
