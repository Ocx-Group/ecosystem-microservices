using MediatR;

namespace Ecosystem.NotificationService.Application.Commands.Brand;

public record UpdateBrandConfigurationCommand(
    string Id,
    long? BrandId,
    string? Name,
    string? SenderName,
    string? SenderEmail,
    string? SupportEmail,
    string? ClientUrl,
    bool? IsActive
) : IRequest<DTOs.BrandConfigurationDto>;
