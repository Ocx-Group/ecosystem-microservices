using Ecosystem.Grpc.Wallet;

namespace Ecosystem.AccountService.Application.Adapters;

public class GrpcWalletServiceAdapter : IWalletServiceAdapter
{
    private readonly WalletGrpc.WalletGrpcClient _client;

    public GrpcWalletServiceAdapter(WalletGrpc.WalletGrpcClient client)
    {
        _client = client;
    }

    public async Task<bool> IsWithdrawalDateAllowed(long brandId)
    {
        var response = await _client.IsWithdrawalDateAllowedAsync(
            new IsWithdrawalDateAllowedRequest { BrandId = brandId });

        return response.Allowed;
    }
}
