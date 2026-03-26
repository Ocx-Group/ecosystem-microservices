namespace Ecosystem.AccountService.Application.Adapters;

public interface IWalletServiceAdapter
{
    Task<bool> IsWithdrawalDateAllowed(long brandId);
}
