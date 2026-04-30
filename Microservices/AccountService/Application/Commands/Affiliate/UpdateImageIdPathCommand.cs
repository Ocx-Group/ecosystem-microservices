using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateImageIdPathCommand : IRequest<UsersAffiliatesDto?>
{
    public int Id { get; init; }

    [JsonPropertyName("image_id_path")]
    public string ImageIdPath { get; init; } = string.Empty;
}
