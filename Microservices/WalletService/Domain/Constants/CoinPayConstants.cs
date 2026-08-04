namespace Ecosystem.WalletService.Domain.Constants;

public static class CoinPayConstants
{
    /// <summary>Named <see cref="System.Net.Http.IHttpClientFactory"/> client pointing at CoinPay's base URL.</summary>
    public const string HttpClientName = "CoinPay";

    /// <summary>Route the provider posts notifications to; excluded from tenant resolution.</summary>
    public const string WebhookPath = "/api/v1/CoinPay/Webhook";

    public const string WithdrawalDetail = "Retiro de fondos en coinpay";
}
