using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Migrate payment strategies from old service
public interface IBalancePaymentStrategy
{
    Task<bool> ExecuteEcoPoolPayment(WalletRequestModel request);
    Task<bool> ExecuteMembershipPayment(WalletRequestModel request);
    Task<bool> ExecuteCustomPayment(WalletRequestModel request);
    Task<bool> ExecutePaymentCourses(WalletRequestModel request);
    Task<bool> ExecuteAdminPayment(WalletRequestModel request);
}
