using Ecosystem.NotificationService.Application.DTOs;
using MediatR;

namespace Ecosystem.NotificationService.Application.Commands.Email;

public record SendEmailCommand(
    string TemplateKey,
    long BrandId,
    string ToEmail,
    string ToName,
    Dictionary<string, string> Placeholders,
    string? SubjectOverride = null,
    List<AttachmentDto>? Attachments = null
) : IRequest<bool>;
