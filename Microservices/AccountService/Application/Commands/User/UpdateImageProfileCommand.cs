using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace Ecosystem.AccountService.Application.Commands.User;

public record UpdateImageProfileCommand : IRequest<UserDto?>
{
    public int Id { get; init; }

    [JsonPropertyName("image_profile_url")]
    public string ImageProfileUrl { get; init; } = string.Empty;
}
