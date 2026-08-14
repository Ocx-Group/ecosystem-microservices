using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.DTOs.MonthlyCommissionDto;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IWalletRepository
{
    Task<List<Wallet>> GetWalletByAffiliateId(int affiliateId, long brandId);
    Task<List<InvoicesDetail>> GetDebitsModel2WithinMonth(DateTime from, DateTime to);
    Task<List<InvoicesDetail>> GetDebitsModel1AWithinMonth(DateTime from, DateTime to);
    Task<List<InvoicesDetail>> GetDebitsModel1BWithinMonth(DateTime from, DateTime to);
    Task<InvoicesSpResponse?> DebitEcoPoolTransactionSp(DebitTransactionRequest request);
    Task<List<InvoicesDetail>> GetDebitsModel2OutsideMonth(DateTime date);
    Task<List<InvoicesDetail>> GetDebitsModel1AOutsideMonth(DateTime date);
    Task<List<InvoicesDetail>> GetDebitsModel1BOutsideMonth(DateTime date);
    Task<List<InvoicesDetail>> GetInvoicesDetailsItemsForModel3(DateTime from, DateTime to);
    Task<List<Wallet>> GetWalletByUserId(int userId, long brandId);
    Task<List<Wallet>> GetWalletsRequest(int userId, long brandId);
    Task<Wallet?> GetWalletById(int id, long brandId);
    Task<bool> CreateModel2Sp(Model2TransactionRequest request);
    Task<bool> CreateModel1ASp(Model1ATransactionRequest request);
    Task<bool> CreateModel1BSp(Model1BTransactionRequest request);
    Task<bool> CreateModel3Sp(Model3TransactionRequest request);
    Task<Wallet> CreateWalletAsync(Wallet request);
    Task<Wallet> UpdateWalletAsync(Wallet request);
    Task<Wallet> DeleteWalletAsync(Wallet request);
    Task<decimal> GetAvailableBalanceByAffiliateId(int userId, long brandId);
    Task<decimal> GetAvailableBalanceAdmin(long brandId);
    Task<decimal?> GetReverseBalanceByAffiliateId(int userId, long brandId);
    Task<decimal?> GetTotalAcquisitionsByAffiliateId(
        int userId,
        long brandId,
        int? paymentGroupId);
    Task<InvoicesSpResponse?> DebitTransaction(DebitTransactionRequest request);
    Task<bool> CreditTransaction(CreditTransactionRequest request);
    Task<bool> CreateTransferBalance(Wallet debitTransaction, Wallet creditTransaction);
    Task<List<Wallet>> GetAllWallets(long brandId);
    Task<List<ModelFourStatistic>> GetUserModelFour(int[] affiliateIds);
    Task<decimal?> GetTotalCommissionsPaid(int affiliateId, long brandId);
    Task<decimal?> GetTotalServiceBalance(int affiliateId, long brandId);
    Task<bool> IsActivePoolGreaterThanOrEqualTo25(int affiliateId, long brandId);
    Task<InvoicesSpResponse?> HandleMembershipTransaction(DebitTransactionRequest request);
    Task<InvoicesSpResponse?> MembershipDebitTransaction(DebitTransactionRequest request);
    Task<InvoicesSpResponse?> AdminDebitTransaction(DebitTransactionRequest request);
    Task<bool> BulkAdministrativeDebitTransaction(Wallet[] requests);

    Task TransactionPoints(int affiliateId, decimal debitLeft, decimal debitRight, decimal creditLeft,
        decimal creditRight);

    Task<IEnumerable<AffiliateBalance>> GetAllAffiliatesWithPositiveBalance(long brandId);
    Task<decimal> GetTotalReverseBalance(long brandId);

    Task<InvoicesSpResponse?> DebitServiceBalanceTransaction(DebitTransactionRequest request);

    Task<InvoicesSpResponse?> DebitServiceBalanceEcoPoolTransactionSp(DebitTransactionRequest request);

    Task<bool> CreditServiceBalanceTransaction(CreditTransactionRequest request);
    Task<List<int>> DistributeCommissionsPerPurchaseAsync(DistributeCommissionsRequest request);

    /// <summary>
    /// Runs <c>calculate_monthly_commissions</c>. With <paramref name="dryRun"/> the
    /// rows describe what would be credited and nothing is written.
    /// </summary>
    Task<List<MonthlyCommissionItemDto>> CalculateMonthlyCommissionsAsync(
        DateOnly startDate,
        DateOnly endDate,
        decimal interestRate,
        int waitingDays,
        int paymentGroupId,
        string adminUserName,
        long brandId,
        bool dryRun);

    /// <summary>
    /// Runs <c>calculate_monthly_commissions_by_invoice</c>, which sums
    /// <c>invoices.total_invoice</c> without joining <c>invoices_details</c> and so takes
    /// no payment group. For brands whose invoices carry no detail rows — see
    /// <c>MonthlyCommissionSources.InvoiceTotal</c>.
    /// </summary>
    Task<List<MonthlyCommissionItemDto>> CalculateMonthlyCommissionsByInvoiceAsync(
        DateOnly startDate,
        DateOnly endDate,
        decimal interestRate,
        int waitingDays,
        string adminUserName,
        long brandId,
        bool dryRun);
    Task<decimal> GetTotalCommissionsPaid(long brandId);
    Task<decimal> GetCommissionsForAdminAsync(long brandId);
    Task<Wallet> CreateAsync(Wallet request);
    Task<decimal?> GetQualificationBalanceAsync(long userId, long brandId);
    Task<List<long>> GetUserIdsWithCommissionsGreaterThanOrEqualTo50(long brandId);
}
