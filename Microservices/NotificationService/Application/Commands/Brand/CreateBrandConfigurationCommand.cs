using MediatR;

namespace Ecosystem.NotificationService.Application.Commands.Brand;

public record CreateBrandConfigurationCommand(
    long BrandId,
    string Name,
    string SenderName,
    string SenderEmail,
    string? SupportEmail,
    string? ClientUrl
) : IRequest<DTOs.BrandConfigurationDto>;
