using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateAffiliateImageCommand(
    int Id,
    [property: JsonPropertyName("image_profile_url")] string ImageProfileUrl
) : IRequest<UsersAffiliatesDto?>;
