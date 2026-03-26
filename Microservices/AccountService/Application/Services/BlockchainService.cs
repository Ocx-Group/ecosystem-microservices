using Ecosystem.AccountService.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Web3;

namespace Ecosystem.AccountService.Application.Services;

public class BlockchainService : IBlockchainService
{
    private readonly Web3? _web3;
    private readonly ILogger<BlockchainService> _logger;

    public BlockchainService(IOptions<BlockchainSettings> settings, ILogger<BlockchainService> logger)
    {
        _logger = logger;
        if (!string.IsNullOrEmpty(settings.Value.BscRpcUrl))
            _web3 = new Web3(settings.Value.BscRpcUrl);
    }

    public Task<bool> IsValidTrc20Address(string address)
    {
        var isValid = !string.IsNullOrEmpty(address)
            && address.Length == 34
            && address.StartsWith("T");

        return Task.FromResult(isValid);
    }

    public async Task<bool> IsValidBscAddress(string address)
    {
        if (string.IsNullOrEmpty(address) || address.Length != 42 || !address.StartsWith("0x"))
            return false;

        if (_web3 is null)
            return true;

        try
        {
            var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
            return balance.Value >= 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to validate BSC address {Address}", address);
            return false;
        }
    }
}
