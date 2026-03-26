using MediatR;

namespace Ecosystem.NotificationService.Application.Commands.Template;

public record DeleteTemplateCommand(string Id) : IRequest<bool>;
