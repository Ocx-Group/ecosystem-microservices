namespace Ecosystem.Domain.Core.Events
{
    /// <summary>
    /// Event published by WalletService when a payment gateway reports that a membership
    /// purchase was cancelled or timed out, so AccountService can undo the activation.
    /// Mirror of <see cref="UpdateActivationDateEvent"/>.
    /// </summary>
    public class RevertActivationDateEvent : Event
    {
        public int AffiliateId { get; set; }
        public long BrandId { get; set; }

        public RevertActivationDateEvent() { }

        public RevertActivationDateEvent(int affiliateId, long brandId)
        {
            AffiliateId = affiliateId;
            BrandId = brandId;
        }
    }
}
