using Ecosystem.WalletService.Domain.Enums;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Adapters;

/// <summary>
/// Credits a purchase paid through an external gateway. The gateway is passed in as
/// <paramref name="paymentMethod"/> so CoinPay, CoinPayments and Pagadito can share
/// one implementation instead of duplicating the crediting rules.
/// </summary>
public interface IExternalPaymentStrategy
{
    Task<bool> ExecuteProductPayment(WalletRequestModel request, CoinPaymentType paymentType, string paymentMethod);
    Task<bool> ExecuteMembershipPayment(WalletRequestModel request, string paymentMethod);
}
