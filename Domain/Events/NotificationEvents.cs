namespace Ecosystem.Domain.Core.Events;

/// <summary>
/// Published by WalletService after a successful product purchase.
/// NotificationService generates the invoice PDF and sends the confirmation email.
/// </summary>
public class SendPaymentConfirmationEvent : Event
{
    public long BrandId { get; set; }
    public string ToEmail { get; set; } = null!;
    public string ToName { get; set; } = null!;
    public InvoiceEventData Invoice { get; set; } = null!;
    public CustomerEventData Customer { get; set; } = null!;
    public List<InvoiceItemEventData> Items { get; set; } = [];

    public SendPaymentConfirmationEvent() { }

    public SendPaymentConfirmationEvent(
        long brandId,
        string toEmail,
        string toName,
        InvoiceEventData invoice,
        CustomerEventData customer,
        List<InvoiceItemEventData> items)
    {
        BrandId = brandId;
        ToEmail = toEmail;
        ToName = toName;
        Invoice = invoice;
        Customer = customer;
        Items = items;
    }
}

/// <summary>
/// Published by WalletService after a successful membership purchase.
/// NotificationService generates the membership PDF, sends welcome + confirmation emails.
/// </summary>
public class SendMembershipConfirmationEvent : Event
{
    public long BrandId { get; set; }
    public string ToEmail { get; set; } = null!;
    public string ToName { get; set; } = null!;
    public InvoiceEventData Invoice { get; set; } = null!;
    public CustomerEventData Customer { get; set; } = null!;

    public SendMembershipConfirmationEvent() { }

    public SendMembershipConfirmationEvent(
        long brandId,
        string toEmail,
        string toName,
        InvoiceEventData invoice,
        CustomerEventData customer)
    {
        BrandId = brandId;
        ToEmail = toEmail;
        ToName = toName;
        Invoice = invoice;
        Customer = customer;
    }
}

/// <summary>
/// Published by WalletService to notify a parent affiliate about a bonus earned.
/// </summary>
public class SendBonusNotificationEvent : Event
{
    public long BrandId { get; set; }
    public string ToEmail { get; set; } = null!;
    public string ToName { get; set; } = null!;
    public string AffiliateUserName { get; set; } = null!;

    public SendBonusNotificationEvent() { }

    public SendBonusNotificationEvent(long brandId, string toEmail, string toName, string affiliateUserName)
    {
        BrandId = brandId;
        ToEmail = toEmail;
        ToName = toName;
        AffiliateUserName = affiliateUserName;
    }
}

// Shared DTOs for notification events
public record InvoiceEventData(
    string ReceiptNumber,
    DateTime Date,
    decimal Total,
    decimal Subtotal,
    decimal TaxTotal);

public record CustomerEventData(
    string Name,
    string LastName,
    string UserName,
    string Email,
    string Phone,
    string Country,
    string City);

public record InvoiceItemEventData(
    string ProductName,
    int Quantity,
    decimal Price,
    decimal Discount,
    decimal Total);
