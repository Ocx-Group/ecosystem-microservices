using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Domain.Services;

public interface IPurchaseBonusService
{
    /// <summary>
    /// Pays the per-purchase bonus to the buyer's upline using the percentages the
    /// brand configured in its own dashboard. A brand with the bonus disabled, with no
    /// levels, or a purchase that did not opt in are all no-ops.
    /// </summary>
    Task DistributeAsync(WalletRequestModel request, decimal invoiceAmount);
}
