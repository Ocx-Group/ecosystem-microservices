using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Replace direct email calls with event bus to NotificationService
public interface IEmailServiceAdapter
{
    Task SendInvitationsForTradingAcademy(UserAffiliateResponse user, string link, string code, long brandId);
    Task SendEmailPurchaseConfirm(UserInfoResponse userInfo, Dictionary<string, byte[]> attachments, InvoicesSpResponse transaction, long brandId);
    Task SendEmailWelcome(UserInfoResponse userInfo, InvoicesSpResponse transaction, long brandId);
    Task SendEmailMembershipConfirm(UserInfoResponse userInfo, byte[] pdfData, InvoicesSpResponse transaction, long brandId);
    Task SendBonusConfirmation(UserInfoResponse bonusWinner, string affiliateUserName, long brandId);
}
