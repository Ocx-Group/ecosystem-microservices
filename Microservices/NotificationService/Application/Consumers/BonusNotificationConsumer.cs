using Ecosystem.Domain.Core.Events;
using Ecosystem.NotificationService.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ecosystem.NotificationService.Application.Consumers;

public class BonusNotificationConsumer : IConsumer<SendBonusNotificationEvent>
{
    private readonly IBrandConfigurationRepository _brandRepository;
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<BonusNotificationConsumer> _logger;

    public BonusNotificationConsumer(
        IBrandConfigurationRepository brandRepository,
        IEmailTemplateRepository templateRepository,
        IEmailService emailService,
        ILogger<BonusNotificationConsumer> logger)
    {
        _brandRepository = brandRepository;
        _templateRepository = templateRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendBonusNotificationEvent> context)
    {
        var msg = context.Message;

        _logger.LogInformation(
            "Processing bonus notification for {ToEmail}, brand={BrandId}",
            msg.ToEmail, msg.BrandId);

        var brand = await _brandRepository.GetByBrandIdAsync(msg.BrandId);
        if (brand is null)
        {
            _logger.LogWarning("Brand {BrandId} not found, skipping bonus notification", msg.BrandId);
            return;
        }

        var template = await _templateRepository.GetByKeyAndBrandAsync("bonus-confirm", msg.BrandId);
        if (template is null)
        {
            _logger.LogWarning("Email template 'bonus-confirm' not found for brand {BrandId}", msg.BrandId);
            return;
        }

        var placeholders = new Dictionary<string, string>
        {
            { "name", msg.ToName },
            { "affiliateUserName", msg.AffiliateUserName }
        };

        var htmlBody = template.HtmlBody;
        foreach (var (key, value) in placeholders)
            htmlBody = htmlBody.Replace($"{{{key}}}", value);

        await _emailService.SendEmailAsync(
            msg.ToEmail, msg.ToName,
            template.Subject, htmlBody,
            brand.SenderName, brand.SenderEmail);
    }
}
