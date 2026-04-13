using Ecosystem.Domain.Core.Events;
using Ecosystem.NotificationService.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ecosystem.NotificationService.Application.Consumers;

public class PaymentConfirmationConsumer : IConsumer<SendPaymentConfirmationEvent>
{
    private readonly IPdfService _pdfService;
    private readonly IBrandConfigurationRepository _brandRepository;
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<PaymentConfirmationConsumer> _logger;

    public PaymentConfirmationConsumer(
        IPdfService pdfService,
        IBrandConfigurationRepository brandRepository,
        IEmailTemplateRepository templateRepository,
        IEmailService emailService,
        ILogger<PaymentConfirmationConsumer> logger)
    {
        _pdfService = pdfService;
        _brandRepository = brandRepository;
        _templateRepository = templateRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendPaymentConfirmationEvent> context)
    {
        var msg = context.Message;

        _logger.LogInformation(
            "Processing payment confirmation for {ToEmail}, brand={BrandId}",
            msg.ToEmail, msg.BrandId);

        var brand = await _brandRepository.GetByBrandIdAsync(msg.BrandId);
        if (brand is null)
        {
            _logger.LogWarning("Brand {BrandId} not found, skipping payment confirmation", msg.BrandId);
            return;
        }

        var template = await _templateRepository.GetByKeyAndBrandAsync("purchase-confirm", msg.BrandId);
        if (template is null)
        {
            _logger.LogWarning("Email template 'purchase-confirm' not found for brand {BrandId}", msg.BrandId);
            return;
        }

        var pdfData = new { invoice = msg.Invoice, customer = msg.Customer, items = msg.Items };
        var pdfBytes = await _pdfService.GenerateFromTemplateAsync("invoice", msg.BrandId, pdfData);

        var placeholders = BuildPlaceholders(msg);
        var htmlBody = ReplacePlaceholders(template.HtmlBody, placeholders);

        var attachments = pdfBytes.Length > 0
            ? [new EmailAttachment("Factura.pdf", pdfBytes)]
            : new List<EmailAttachment>();

        await _emailService.SendEmailAsync(
            msg.ToEmail,
            msg.ToName,
            template.Subject,
            htmlBody,
            brand.SenderName,
            brand.SenderEmail,
            attachments);
    }

    private static Dictionary<string, string> BuildPlaceholders(SendPaymentConfirmationEvent msg) => new()
    {
        { "name", msg.Customer.Name },
        { "lastName", msg.Customer.LastName },
        { "receiptNumber", msg.Invoice.ReceiptNumber },
        { "total", msg.Invoice.Total.ToString("N2") },
        { "date", msg.Invoice.Date.ToString("yyyy-MM-dd") }
    };

    private static string ReplacePlaceholders(string html, Dictionary<string, string> placeholders)
    {
        foreach (var (key, value) in placeholders)
            html = html.Replace($"{{{key}}}", value);
        return html;
    }
}
