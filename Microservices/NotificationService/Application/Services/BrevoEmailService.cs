using Ecosystem.NotificationService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace Ecosystem.NotificationService.Application.Services;

public class BrevoEmailService : IEmailService
{
    private readonly TransactionalEmailsApi _apiInstance;
    private readonly ILogger<BrevoEmailService> _logger;

    public BrevoEmailService(IConfiguration configuration, ILogger<BrevoEmailService> logger)
    {
        _logger = logger;
        var apiKey = configuration["Brevo:ApiKey"]
            ?? throw new InvalidOperationException("Brevo:ApiKey is not configured");

        Configuration.Default.AddApiKey("api-key", apiKey);
        _apiInstance = new TransactionalEmailsApi();
    }

    public async Task<bool> SendEmailAsync(
        string toEmail,
        string toName,
        string subject,
        string htmlBody,
        string senderName,
        string senderEmail,
        List<EmailAttachment>? attachments = null)
    {
        try
        {
            var sender = new SendSmtpEmailSender(senderName, senderEmail);
            var to = new List<SendSmtpEmailTo> { new(toEmail, toName) };

            var email = new SendSmtpEmail(
                sender: sender,
                to: to,
                subject: subject,
                htmlContent: htmlBody);

            if (attachments is { Count: > 0 })
            {
                email.Attachment = attachments
                    .Select(a => new SendSmtpEmailAttachment(
                        content: a.Content,
                        name: a.FileName))
                    .ToList();
            }

            await _apiInstance.SendTransacEmailAsync(email);

            _logger.LogInformation("Email sent successfully to {ToEmail} with subject '{Subject}'",
                toEmail, subject);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {ToEmail} with subject '{Subject}'",
                toEmail, subject);
            return false;
        }
    }
}
