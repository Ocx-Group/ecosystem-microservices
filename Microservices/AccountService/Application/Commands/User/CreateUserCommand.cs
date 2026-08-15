using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.Commands.User;

public record CreateUserCommand : IRequest<UserDto?>
{
    [JsonProperty("rol_id")] public int RolId { get; init; }

    [JsonProperty("user_name")] public string? UserName { get; init; }

    [JsonProperty("email")] public string? Email { get; init; }

    [JsonProperty("password")] public string? Password { get; init; }

    [JsonProperty("name")] public string? Name { get; init; }

    [JsonProperty("last_name")] public string? LastName { get; init; }

    [JsonProperty("phone")] public string? Phone { get; init; }

    [JsonProperty("address")] public string? Address { get; init; }

    [JsonProperty("observation")] public string? Observation { get; init; }

    [JsonProperty("status")] public bool Status { get; init; }
}
