using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Responses;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Domain.Services;

public interface IPaymentNotificationService
{
    Task SendPaymentConfirmation(
        UserInfoResponse userInfo,
        InvoicesSpResponse transactionResponse,
        WalletRequestModel request,
        List<InvoiceDetailsTransactionRequest> invoiceDetails);

    Task SendMembershipConfirmation(
        UserInfoResponse userInfo,
        InvoicesSpResponse transactionResponse,
        WalletRequestModel request);

    Task SendBonusNotification(
        UserInfoResponse bonusWinner,
        string affiliateUserName,
        long brandId);
}
