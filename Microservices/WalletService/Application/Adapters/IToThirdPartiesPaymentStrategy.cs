using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Migrate payment strategies from old service
public interface IToThirdPartiesPaymentStrategy
{
    Task<bool> ExecutePayment(WalletRequestModel request);
}
