using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;

/// <summary>
/// A withdrawal request selected in the admin panel. The clients POST their full
/// <c>WalletRequestRequest</c> rows; only these three fields are consumed, the rest is ignored.
/// </summary>
public class CoinPaymentsWithdrawalRequest
{
    /// <summary>Identifier of the wallets_requests row, marked completed once the payout lands.</summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("affiliateId")]
    public int AffiliateId { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
}
