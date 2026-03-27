using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.DTOs.PaginationDto;
using Ecosystem.WalletService.Domain.Requests.PaginationRequest;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IInvoiceRepository
{
    Task<PaginationDto<Invoice>> GetAllInvoices(long brandId, PaginationRequest request);
    Task<int> CountDetailsByPaymentGroup(int paymentGroupId, int userId, long brandId);
    Task<List<ModelFourStatistic>> Model4StatisticsByUser(int userId);
    Task<List<Invoice>> GetAllInvoicesUser(int id, long brandId);
    Task<Invoice> CreateInvoiceAsync(Invoice invoice);
    Task<Invoice?> GetInvoiceById(long id, long brandId);
    Task<Invoice> DeleteInvoiceAsync(Invoice invoice);
    Task<InvoicesSpResponse?> HandleDebitTransaction(DebitTransactionRequest request);
    Task<Invoice?> GetInvoiceByReceiptNumber(string receiptNumber, long brandId);
    Task<List<Invoice>> GetInvoicesByReceiptNumber(ICollection<string> transactionIds);
    Task RevertCoinPaymentTransactions(List<InvoiceNumber> invoiceNumbers);
    Task<bool> InvoiceExistsByReceiptNumber(string idTransaction, long brandId);
    Task<bool> GetInvoicesForTradingAcademyPurchases(int affiliateId);
    Task<List<Invoice>> DeleteMultipleInvoicesAndDetailsAsync(long[] invoiceIds, long brandId);
    Task<List<InvoicesTradingAcademyResponse>?> GetAllInvoicesForTradingAcademyPurchases();
    Task<List<InvoiceModelOneAndTwoResponse>?> GetAllInvoicesModelOneAndTwo();
    Task<int> CountDetailsModel3ByPaymentGroup(int userId, long brandId);
    Task<decimal> GetTotalAdquisitionsAdmin(long brandId, int paymentGroupId);
    IAsyncEnumerable<List<Invoice>> GetInvoicesInBatches(DateTime? startDate, DateTime? endDate, int batchSize, long brandId);
}