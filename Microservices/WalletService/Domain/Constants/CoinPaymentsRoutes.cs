namespace Ecosystem.WalletService.Domain.Constants;

/// <summary>
/// coinpayments.net exposes a single endpoint; the operation travels in the "cmd" form field.
/// </summary>
public static class CoinPaymentsRoutes
{
    public const string ApiPath = "/api.php";

    public const string GetProfileCommand = "get_pbn_info";
    public const string GetDepositAddressCommand = "get_deposit_address";
    public const string GetBalancesCommand = "balances";
    public const string CreateTransactionCommand = "create_transaction";
    public const string GetTransactionInfoCommand = "get_tx_info";
    public const string CreateMassWithdrawalCommand = "create_mass_withdrawal";
}
