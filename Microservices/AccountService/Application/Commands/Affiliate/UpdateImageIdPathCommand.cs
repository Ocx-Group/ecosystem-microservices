using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateImageIdPathCommand(
    int Id,
    [property: JsonPropertyName("image_id_path")] string ImageIdPath
) : IRequest<UsersAffiliatesDto?>;
