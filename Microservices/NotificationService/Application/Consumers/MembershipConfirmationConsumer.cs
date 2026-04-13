using Ecosystem.Domain.Core.Events;
using Ecosystem.NotificationService.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ecosystem.NotificationService.Application.Consumers;

public class MembershipConfirmationConsumer : IConsumer<SendMembershipConfirmationEvent>
{
    private readonly IPdfService _pdfService;
    private readonly IBrandConfigurationRepository _brandRepository;
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<MembershipConfirmationConsumer> _logger;

    public MembershipConfirmationConsumer(
        IPdfService pdfService,
        IBrandConfigurationRepository brandRepository,
        IEmailTemplateRepository templateRepository,
        IEmailService emailService,
        ILogger<MembershipConfirmationConsumer> logger)
    {
        _pdfService = pdfService;
        _brandRepository = brandRepository;
        _templateRepository = templateRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendMembershipConfirmationEvent> context)
    {
        var msg = context.Message;

        _logger.LogInformation(
            "Processing membership confirmation for {ToEmail}, brand={BrandId}",
            msg.ToEmail, msg.BrandId);

        var brand = await _brandRepository.GetByBrandIdAsync(msg.BrandId);
        if (brand is null)
        {
            _logger.LogWarning("Brand {BrandId} not found, skipping membership confirmation", msg.BrandId);
            return;
        }

        // 1. Send welcome email
        var welcomeTemplate = await _templateRepository.GetByKeyAndBrandAsync("welcome", msg.BrandId);
        if (welcomeTemplate is not null)
        {
            var welcomePlaceholders = new Dictionary<string, string>
            {
                { "name", msg.Customer.Name },
                { "lastName", msg.Customer.LastName },
                { "userName", msg.Customer.UserName }
            };
            var welcomeHtml = ReplacePlaceholders(welcomeTemplate.HtmlBody, welcomePlaceholders);

            await _emailService.SendEmailAsync(
                msg.ToEmail, msg.ToName,
                welcomeTemplate.Subject, welcomeHtml,
                brand.SenderName, brand.SenderEmail);
        }

        // 2. Generate membership PDF and send confirmation email
        var confirmTemplate = await _templateRepository.GetByKeyAndBrandAsync("membership-confirm", msg.BrandId);
        if (confirmTemplate is null)
        {
            _logger.LogWarning("Email template 'membership-confirm' not found for brand {BrandId}", msg.BrandId);
            return;
        }

        var pdfData = new { invoice = msg.Invoice, customer = msg.Customer };
        var pdfBytes = await _pdfService.GenerateFromTemplateAsync("membership", msg.BrandId, pdfData);

        var placeholders = new Dictionary<string, string>
        {
            { "name", msg.Customer.Name },
            { "lastName", msg.Customer.LastName },
            { "receiptNumber", msg.Invoice.ReceiptNumber },
            { "total", msg.Invoice.Total.ToString("N2") },
            { "date", msg.Invoice.Date.ToString("yyyy-MM-dd") }
        };
        var htmlBody = ReplacePlaceholders(confirmTemplate.HtmlBody, placeholders);

        var attachments = pdfBytes.Length > 0
            ? [new EmailAttachment("Membresia.pdf", pdfBytes)]
            : new List<EmailAttachment>();

        await _emailService.SendEmailAsync(
            msg.ToEmail, msg.ToName,
            confirmTemplate.Subject, htmlBody,
            brand.SenderName, brand.SenderEmail,
            attachments);
    }

    private static string ReplacePlaceholders(string html, Dictionary<string, string> placeholders)
    {
        foreach (var (key, value) in placeholders)
            html = html.Replace($"{{{key}}}", value);
        return html;
    }
}
