using MediatR;

namespace Ecosystem.NotificationService.Application.Commands.Template;

public record UpdateTemplateCommand(
    long Id,
    string? TemplateKey,
    string? Subject,
    string? HtmlBody,
    List<string>? Placeholders,
    bool? IsActive
) : IRequest<DTOs.EmailTemplateDto>;
