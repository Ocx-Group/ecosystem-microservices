using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.ValueObjects;

namespace Ecosystem.WalletService.Domain.Services;

public interface IDebitTransactionBuilder
{
    IDebitTransactionBuilder ForAffiliate(int affiliateId, string affiliateUserName);
    IDebitTransactionBuilder WithConcept(string concept, string conceptType);
    IDebitTransactionBuilder WithAmounts(PaymentCalculation calculation);
    IDebitTransactionBuilder WithPaymentMethod(string paymentMethod);
    IDebitTransactionBuilder WithInvoiceDetails(List<InvoiceDetailsTransactionRequest> invoiceDetails);
    IDebitTransactionBuilder WithReceiptInfo(string? bank, string? receiptNumber, bool type);
    IDebitTransactionBuilder WithCommissionCalculation(bool? includeInCommissionCalculation);
    Task<DebitTransactionRequest> BuildAsync(long brandId);
    IDebitTransactionBuilder Reset();
}
