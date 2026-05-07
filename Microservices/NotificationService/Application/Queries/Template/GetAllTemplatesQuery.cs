using Ecosystem.NotificationService.Application.DTOs;
using MediatR;

namespace Ecosystem.NotificationService.Application.Queries.Template;

public record GetAllTemplatesQuery() : IRequest<ICollection<EmailTemplateDto>>;
