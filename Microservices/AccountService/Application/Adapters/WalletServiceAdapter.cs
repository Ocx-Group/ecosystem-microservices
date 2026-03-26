namespace Ecosystem.AccountService.Application.Adapters;

/// <summary>
/// Placeholder implementation. Wire up to actual WalletService HTTP endpoint when available.
/// </summary>
public class WalletServiceAdapter : IWalletServiceAdapter
{
    public Task<bool> IsWithdrawalDateAllowed(long brandId)
    {
        // TODO: Implement HTTP call to WalletService to check withdrawal periods
        return Task.FromResult(true);
    }
}
