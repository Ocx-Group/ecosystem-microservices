namespace Ecosystem.Domain.Core.Events
{
    /// <summary>
    /// Event published by any microservice to request an email be sent via NotificationService.
    /// </summary>
    public class SendEmailEvent : Event
    {
        public string TemplateKey { get; set; } = null!;

        public long BrandId { get; set; }

        public string ToEmail { get; set; } = null!;

        public string ToName { get; set; } = null!;

        public Dictionary<string, string> Placeholders { get; set; } = new();

        public string? SubjectOverride { get; set; }

        public SendEmailEvent() { }

        public SendEmailEvent(
            string templateKey,
            long brandId,
            string toEmail,
            string toName,
            Dictionary<string, string> placeholders,
            string? subjectOverride = null)
        {
            TemplateKey = templateKey;
            BrandId = brandId;
            ToEmail = toEmail;
            ToName = toName;
            Placeholders = placeholders;
            SubjectOverride = subjectOverride;
        }
    }
}
