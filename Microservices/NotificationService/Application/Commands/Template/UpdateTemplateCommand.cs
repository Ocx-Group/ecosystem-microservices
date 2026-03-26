using MediatR;

namespace Ecosystem.NotificationService.Application.Commands.Template;

public record UpdateTemplateCommand(
    string Id,
    string? TemplateKey,
    long? BrandId,
    string? Subject,
    string? HtmlBody,
    List<string>? Placeholders,
    bool? IsActive
) : IRequest<DTOs.EmailTemplateDto>;
