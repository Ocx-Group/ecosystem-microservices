using MediatR;

namespace Ecosystem.NotificationService.Application.Commands.Template;

public record DeleteTemplateCommand(long Id) : IRequest<bool>;
