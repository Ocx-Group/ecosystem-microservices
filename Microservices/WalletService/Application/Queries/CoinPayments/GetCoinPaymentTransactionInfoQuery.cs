using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.CoinPayments;

public record GetCoinPaymentTransactionInfoQuery(string TxnId) : IRequest<GetTransactionInfoResponse?>;
