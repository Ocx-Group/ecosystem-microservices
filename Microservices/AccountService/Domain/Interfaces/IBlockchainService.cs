namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IBlockchainService
{
    Task<bool> IsValidTrc20Address(string address);
    Task<bool> IsValidBscAddress(string address);
}
