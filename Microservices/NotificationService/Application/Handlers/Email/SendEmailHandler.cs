using System.Text;
using Ecosystem.NotificationService.Application.Commands.Email;
using Ecosystem.NotificationService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.NotificationService.Application.Handlers.Email;

public class SendEmailHandler : IRequestHandler<SendEmailCommand, bool>
{
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly IBrandConfigurationRepository _brandRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailHandler> _logger;

    public SendEmailHandler(
        IEmailTemplateRepository templateRepository,
        IBrandConfigurationRepository brandRepository,
        IEmailService emailService,
        ILogger<SendEmailHandler> logger)
    {
        _templateRepository = templateRepository;
        _brandRepository = brandRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<bool> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByKeyAndBrandAsync(request.TemplateKey, request.BrandId)
            ?? throw new KeyNotFoundException(
                $"Template '{request.TemplateKey}' not found for brand {request.BrandId}");

        var brand = await _brandRepository.GetByBrandIdAsync(request.BrandId)
            ?? throw new KeyNotFoundException($"Brand configuration not found for brand {request.BrandId}");

        var htmlBody = ReplacePlaceholders(template.HtmlBody, request.Placeholders);
        var subject = request.SubjectOverride ?? template.Subject;

        var attachments = request.Attachments?
            .Select(a => new EmailAttachment(a.FileName, a.Content, a.ContentType))
            .ToList();

        _logger.LogInformation(
            "Sending email: template={TemplateKey}, brand={BrandId}, to={ToEmail}",
            request.TemplateKey, request.BrandId, request.ToEmail);

        return await _emailService.SendEmailAsync(
            request.ToEmail,
            request.ToName,
            subject,
            htmlBody,
            brand.SenderName,
            brand.SenderEmail,
            attachments);
    }

    private static string ReplacePlaceholders(string html, Dictionary<string, string> placeholders)
    {
        var sb = new StringBuilder(html);
        foreach (var (key, value) in placeholders)
        {
            sb.Replace($"{{{key}}}", value);
        }
        return sb.ToString();
    }
}
