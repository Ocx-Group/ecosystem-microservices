using Ecosystem.WalletService.Domain.Enums;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Adapters;

public interface ICoinPaymentsPaymentStrategy
{
    Task<bool> ExecuteProductPayment(WalletRequestModel request, CoinPaymentType paymentType);
    Task<bool> ExecuteMembershipPayment(WalletRequestModel request);
}
