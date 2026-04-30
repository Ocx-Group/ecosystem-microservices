using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateAffiliateImageCommand : IRequest<UsersAffiliatesDto?>
{
    public int Id { get; init; }

    [JsonProperty("image_profile_url")]
    public string ImageProfileUrl { get; init; } = string.Empty;
}
