using MediatR;

namespace Ecosystem.NotificationService.Application.Commands.Template;

public record CreateTemplateCommand(
    string TemplateKey,
    string Subject,
    string HtmlBody,
    List<string> Placeholders
) : IRequest<DTOs.EmailTemplateDto>;
