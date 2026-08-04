using MediatR;

namespace Ecosystem.WalletService.Application.Queries.CoinPay;

/// <summary>
/// Polled by the front end while a deposit is pending: true once the local
/// transaction for that reference has been credited.
/// </summary>
public record GetCoinPayTransactionByReferenceQuery(string Reference) : IRequest<bool>;
