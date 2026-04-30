using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace Ecosystem.AccountService.Application.Commands.User;

public record UpdateImageProfileCommand(
    int Id,
    [property: JsonPropertyName("image_profile_url")] string ImageProfileUrl
) : IRequest<UserDto?>;
