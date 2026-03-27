namespace Ecosystem.WalletService.Domain.Constants;

public static class CoinPayRoutes
{
    public const string GetTransactionRoute = "/api/integration/transaction/getTransaction/{0}/v1";
    public const string CreateTransactionRoute = "/api/integration/cpay/requestPayment/v1";
    public const string SendFundsRoute = "/api/integration/wallet/sendFunds/v1";
    public const string CreateChannelRoute = "/api/integration/wallet/createChannel/v1";
    public const string GetNetworksByIdCurrencyRoute = "api/integration/wallet/getNetwords/{0}/v1";
    public const string CreateAddressRoute = "api/integration/wallet/createAddress/{0}/v1";
}