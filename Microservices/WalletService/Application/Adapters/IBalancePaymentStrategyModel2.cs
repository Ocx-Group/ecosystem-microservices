using Ecosystem.WalletService.Domain.Requests.WalletRequest;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Migrate payment strategies from old service
public interface IBalancePaymentStrategyModel2
{
    Task<bool> ExecutePayment(WalletRequest request);
}
