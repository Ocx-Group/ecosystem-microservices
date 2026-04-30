using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.Commands.User;

public record UpdateImageProfileCommand : IRequest<UserDto?>
{
    public int Id { get; init; }

    [JsonProperty("image_profile_url")]
    public string ImageProfileUrl { get; init; } = string.Empty;
}
