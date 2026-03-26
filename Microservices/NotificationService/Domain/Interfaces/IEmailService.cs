namespace Ecosystem.NotificationService.Domain.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(
        string toEmail,
        string toName,
        string subject,
        string htmlBody,
        string senderName,
        string senderEmail,
        List<EmailAttachment>? attachments = null);
}

public record EmailAttachment(string FileName, byte[] Content, string ContentType = "application/pdf");
