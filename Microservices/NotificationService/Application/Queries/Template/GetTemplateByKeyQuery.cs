using Ecosystem.NotificationService.Application.DTOs;
using MediatR;

namespace Ecosystem.NotificationService.Application.Queries.Template;

public record GetTemplateByKeyQuery(string TemplateKey, long BrandId) : IRequest<EmailTemplateDto?>;
