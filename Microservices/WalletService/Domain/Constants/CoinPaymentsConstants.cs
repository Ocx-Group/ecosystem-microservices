namespace Ecosystem.WalletService.Domain.Constants;

public static class CoinPaymentsConstants
{
    /// <summary>Named <see cref="System.Net.Http.IHttpClientFactory"/> client pointing at coinpayments.net.</summary>
    public const string HttpClientName = "CoinPayments";

    /// <summary>Route the provider posts payment notifications to; excluded from tenant resolution.</summary>
    public const string IpnPath = "/api/v1/ConPayments/coinPaymentsIPN";

    /// <summary>Second notification route, used by the matrix activation flow.</summary>
    public const string MatrixIpnPath = "/api/v1/MatrixQualification/coinpayments_matrix_activation_confirmation";

    /// <summary>Header carrying the HMAC-SHA512 digest of the raw IPN body.</summary>
    public const string SignatureHeader = "Hmac";

    public const string IpnModeHmac = "hmac";

    public const string IpnTypeApi = "api";

    /// <summary>Value the provider returns in "error" when a call succeeded.</summary>
    public const string SuccessError = "ok";

    /// <summary>
    /// Value written to transactions.payment_method. Deliberately singular and distinct from
    /// <see cref="Constants.CoinPayments"/>: it is the literal the old service persisted, and
    /// historical rows are queried by it.
    /// </summary>
    public const string TransactionPaymentMethod = "Coinpayment";

    public const string WithdrawalDetail = "Retiro de fondos en coinpayments";

    /// <summary>Surcharge added to the requested amount before quoting the provider.</summary>
    public const decimal FeeRate = 0.01m;

    /// <summary>Payment method id recorded on the wallet request for CoinPayments purchases.</summary>
    public const int PaymentMethodId = 4;

    /// <summary>Product id that identifies a membership purchase.</summary>
    public const int MembershipProductId = 1;

    /// <summary>Status the provider reports while a transaction is waiting for funds.</summary>
    public const int WaitingStatusCode = 0;

    /// <summary>Status reported once funds arrived but confirmations are still pending.</summary>
    public const int FundsReceivedStatusCode = 1;

    /// <summary>Currencies accepted on an incoming IPN.</summary>
    public static readonly string[] ValidCurrencies =
    [
        Constants.ConPaymentCurrency,
        Constants.CoinPaymentsBnbCurrency
    ];
}
