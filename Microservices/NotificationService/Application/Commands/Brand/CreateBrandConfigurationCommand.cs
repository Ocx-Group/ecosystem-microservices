using MediatR;

namespace Ecosystem.NotificationService.Application.Commands.Brand;

public record CreateBrandConfigurationCommand(
    string Name,
    string SenderName,
    string SenderEmail,
    string? SupportEmail,
    string? ClientUrl
) : IRequest<DTOs.BrandConfigurationDto>;
