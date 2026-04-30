using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateAffiliateImageCommand : IRequest<UsersAffiliatesDto?>
{
    public int Id { get; init; }

    [JsonPropertyName("image_profile_url")]
    public string ImageProfileUrl { get; init; } = string.Empty;
}
