using Ecosystem.WalletService.Domain.Responses;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Domain.Services;

public interface IMembershipBonusService
{
    Task CreditBonusToParentAsync(UserInfoResponse userInfo, WalletRequestModel request);
}
