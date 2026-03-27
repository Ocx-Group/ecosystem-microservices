using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Migrate payment strategies from old service
public interface IBalancePaymentStrategyModel1B
{
    Task<bool> ExecuteEcoPoolPayment(WalletRequestModel request);
    Task<bool> ExecuteEcoPoolPaymentWithServiceBalance(WalletRequestModel request);
}
