using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.CoinPayments;

public record GetCoinPaymentsProfileQuery : IRequest<GetBasicInfoResponse?>;
