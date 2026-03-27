using Ecosystem.WalletService.Domain.Requests.WalletRequest;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Migrate payment strategies from old service
public interface IBalancePaymentStrategy
{
    Task<bool> ExecuteEcoPoolPayment(WalletRequest request);
    Task<bool> ExecuteMembershipPayment(WalletRequest request);
    Task<bool> ExecuteCustomPayment(WalletRequest request);
    Task<bool> ExecutePaymentCourses(WalletRequest request);
    Task<bool> ExecuteAdminPayment(WalletRequest request);
}
