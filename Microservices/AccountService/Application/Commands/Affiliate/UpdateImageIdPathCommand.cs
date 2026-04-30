using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateImageIdPathCommand : IRequest<UsersAffiliatesDto?>
{
    public int Id { get; init; }

    [JsonProperty("image_id_path")]
    public string ImageIdPath { get; init; } = string.Empty;
}
