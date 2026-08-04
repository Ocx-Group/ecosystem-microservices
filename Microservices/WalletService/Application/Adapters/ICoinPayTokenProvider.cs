namespace Ecosystem.WalletService.Application.Adapters;

/// <summary>
/// Obtains and caches the short lived bearer token issued by CoinPay's
/// integration auth endpoint. Registered as a singleton so the token is shared
/// across requests instead of being re-issued on every outbound call.
/// </summary>
public interface ICoinPayTokenProvider
{
    Task<string?> GetTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>Drops the cached token so the next call re-authenticates.</summary>
    void Invalidate();
}
