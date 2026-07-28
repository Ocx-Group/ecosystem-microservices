using System.Text;
using Ecosystem.Domain.Core.Events;
using Ecosystem.NotificationService.Application.Adapters;
using Ecosystem.NotificationService.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ecosystem.NotificationService.Application.Consumers;

public class SendEmailConsumer : IConsumer<SendEmailEvent>
{
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly IBrandConfigurationReader _brandConfigurationReader;
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailConsumer> _logger;

    public SendEmailConsumer(
        IEmailTemplateRepository templateRepository,
        IBrandConfigurationReader brandConfigurationReader,
        IEmailService emailService,
        ILogger<SendEmailConsumer> logger)
    {
        _templateRepository = templateRepository;
        _brandConfigurationReader = brandConfigurationReader;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendEmailEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Received SendEmailEvent: template={TemplateKey}, brand={BrandId}, to={ToEmail}",
            message.TemplateKey, message.BrandId, message.ToEmail);

        var template = await _templateRepository.GetByKeyAndBrandAsync(message.TemplateKey, message.BrandId);
        if (template is null)
        {
            _logger.LogWarning("Template '{TemplateKey}' not found for brand {BrandId}",
                message.TemplateKey, message.BrandId);
            return;
        }

        var brand = await _brandConfigurationReader.GetByBrandIdAsync(
            message.BrandId,
            context.CancellationToken);
        if (brand is null)
        {
            _logger.LogWarning("Brand configuration not found for brand {BrandId}", message.BrandId);
            return;
        }

        var placeholders = new Dictionary<string, string>(message.Placeholders)
        {
            ["brandName"]    = brand.Name,
            ["clientUrl"]    = brand.ClientUrl,
            ["supportEmail"] = string.IsNullOrWhiteSpace(brand.SupportEmail)
                ? brand.SenderEmail
                : brand.SupportEmail,
            ["senderName"]   = brand.SenderName
        };

        var htmlBody = ReplacePlaceholders(template.HtmlBody, placeholders);
        var subject = ReplacePlaceholders(message.SubjectOverride ?? template.Subject, placeholders);

        await _emailService.SendEmailAsync(
            message.ToEmail,
            message.ToName,
            subject,
            htmlBody,
            brand.SenderName,
            brand.SenderEmail);
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
