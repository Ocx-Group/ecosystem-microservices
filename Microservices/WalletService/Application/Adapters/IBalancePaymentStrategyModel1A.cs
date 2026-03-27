using Ecosystem.WalletService.Domain.Requests.WalletRequest;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Migrate payment strategies from old service
public interface IBalancePaymentStrategyModel1A
{
    Task<bool> ExecuteEcoPoolPayment(WalletRequest request);
    Task<bool> ExecuteEcoPoolPaymentWithServiceBalance(WalletRequest request);
}
