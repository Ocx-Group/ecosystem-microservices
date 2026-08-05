using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Application.Adapters;

public interface ICoinPaymentsAdapter
{
    Task<GetBasicInfoResponse?> GetProfile(string pbnTag);
    Task<GetDepositAddressResponse?> GetDepositAddress(string currency);
    Task<GetCoinBalancesResponse?> GetCoinBalances(bool includeZeroBalances);
    Task<CreateConPaymentsTransactionResponse?> CreatePayment(ConPaymentRequest request);
    Task<GetTransactionInfoResponse?> GetTransactionInfo(string txnId, bool full);
    Task<CoinPaymentWithdrawalResponse?> CreateMassWithdrawal(IEnumerable<CoinPaymentMassWithdrawalRequest> requests);

    /// <summary>
    /// Verifies the HMAC CoinPayments attaches to an IPN. <paramref name="rawBody"/> must be the
    /// untouched request body — the digest is taken over the bytes exactly as they arrived.
    /// </summary>
    bool VerifyIpnSignature(string rawBody, string? signature);
}
