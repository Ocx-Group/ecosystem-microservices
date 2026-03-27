using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.CoinPay;

public record GetCoinPayTransactionByIdQuery(int TransactionId) : IRequest<GetTransactionByIdResponse?>;
