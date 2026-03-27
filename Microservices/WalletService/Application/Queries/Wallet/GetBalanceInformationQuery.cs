using Ecosystem.WalletService.Domain.DTOs.BalanceInformationDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Wallet;

public record GetBalanceInformationQuery(int AffiliateId) : IRequest<BalanceInformationDto>;
