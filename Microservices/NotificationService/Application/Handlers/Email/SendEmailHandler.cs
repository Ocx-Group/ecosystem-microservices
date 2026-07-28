using System.Text;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.NotificationService.Application.Adapters;
using Ecosystem.NotificationService.Application.Commands.Email;
using Ecosystem.NotificationService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.NotificationService.Application.Handlers.Email;

public class SendEmailHandler : IRequestHandler<SendEmailCommand, bool>
{
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly IBrandConfigurationReader _brandConfigurationReader;
    private readonly IEmailService _emailService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<SendEmailHandler> _logger;

    public SendEmailHandler(
        IEmailTemplateRepository templateRepository,
        IBrandConfigurationReader brandConfigurationReader,
        IEmailService emailService,
        ITenantContext tenantContext,
        ILogger<SendEmailHandler> logger)
    {
        _templateRepository = templateRepository;
        _brandConfigurationReader = brandConfigurationReader;
        _emailService = emailService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;

        var template = await _templateRepository.GetByKeyAndBrandAsync(request.TemplateKey, brandId)
            ?? throw new KeyNotFoundException(
                $"Template '{request.TemplateKey}' not found for brand {brandId}");

        var brand = await _brandConfigurationReader.GetByBrandIdAsync(brandId, cancellationToken)
            ?? throw new KeyNotFoundException($"Brand configuration not found for brand {brandId}");

        var placeholders = new Dictionary<string, string>(request.Placeholders)
        {
            ["brandName"]    = brand.Name,
            ["clientUrl"]    = brand.ClientUrl,
            ["supportEmail"] = string.IsNullOrWhiteSpace(brand.SupportEmail)
                ? brand.SenderEmail
                : brand.SupportEmail,
            ["senderName"]   = brand.SenderName,
        };

        var htmlBody = ReplacePlaceholders(template.HtmlBody, placeholders);
        var subject = ReplacePlaceholders(request.SubjectOverride ?? template.Subject, placeholders);

        var attachments = request.Attachments?
            .Select(a => new EmailAttachment(a.FileName, a.Content, a.ContentType))
            .ToList();

        _logger.LogInformation(
            "Sending email: template={TemplateKey}, brand={BrandId}, to={ToEmail}",
            request.TemplateKey, brandId, request.ToEmail);

        return await _emailService.SendEmailAsync(
            request.ToEmail,
            request.ToName,
            subject,
            htmlBody,
            brand.SenderName,
            brand.SenderEmail,
            attachments);
    }

    private static string ReplacePlaceholders(string source, Dictionary<string, string> placeholders)
    {
        var sb = new StringBuilder(source);
        foreach (var (key, value) in placeholders)
        {
            sb.Replace($"{{{key}}}", value);
        }
        return sb.ToString();
    }
}
