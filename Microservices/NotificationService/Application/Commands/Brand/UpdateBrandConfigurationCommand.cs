using MediatR;

namespace Ecosystem.NotificationService.Application.Commands.Brand;

public record UpdateBrandConfigurationCommand(
    long Id,
    long? BrandId,
    string? Name,
    string? SenderName,
    string? SenderEmail,
    string? SupportEmail,
    string? ClientUrl,
    bool? IsActive
) : IRequest<DTOs.BrandConfigurationDto>;
