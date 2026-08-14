namespace Ecosystem.Domain.Core.BrandConfiguration;

/// <summary>
/// Which stored procedure liquidates a brand's monthly commission. Stored as text in
/// <c>brand_configuration.monthly_commission_source</c> and compared case-insensitively,
/// following the same shape as <c>withdrawal_validation_type</c>.
///
/// The two procedures are not interchangeable at runtime: the payment-group one reaches
/// invoices through <c>invoices_details</c>, so an invoice with no detail rows is
/// invisible to it regardless of the payment group asked for. Picking the source is
/// therefore a property of the brand's data, not of a single run.
/// </summary>
public static class MonthlyCommissionSources
{
    /// <summary>
    /// <c>wallet_service.calculate_monthly_commissions</c> — sums the invoices whose
    /// details belong to the brand's configured payment group. The default, and what
    /// Ecosystem, RecyCoin and HouseCoin use.
    /// </summary>
    public const string PaymentGroup = "PaymentGroup";

    /// <summary>
    /// <c>wallet_service.calculate_monthly_commissions_by_invoice</c> — sums
    /// <c>invoices.total_invoice</c> directly and never joins the details, so the
    /// payment group plays no part. Used by RecyBot, whose invoices were created by the
    /// RecyCoin migration without any detail rows.
    /// </summary>
    public const string InvoiceTotal = "InvoiceTotal";

    public static bool IsValid(string? source)
        => Matches(source, PaymentGroup) || Matches(source, InvoiceTotal);

    /// <summary>
    /// True when the brand liquidates on the invoice total. Anything else — including a
    /// null or an unrecognised value — falls back to the payment-group procedure, which
    /// is the conservative side: it pays a filtered subset rather than every invoice.
    /// </summary>
    public static bool IsInvoiceTotal(string? source) => Matches(source, InvoiceTotal);

    private static bool Matches(string? source, string expected)
        => string.Equals(source, expected, StringComparison.OrdinalIgnoreCase);
}
