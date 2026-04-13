namespace Ecosystem.Domain.Core.Events;

/// <summary>
/// Event published by WalletService after a membership purchase
/// to notify AccountService to update the affiliate's activation date.
/// </summary>
public class UpdateActivationDateEvent : Event
{
    public int AffiliateId { get; set; }
    public long BrandId { get; set; }

    public UpdateActivationDateEvent() { }

    public UpdateActivationDateEvent(int affiliateId, long brandId)
    {
        AffiliateId = affiliateId;
        BrandId = brandId;
    }
}
