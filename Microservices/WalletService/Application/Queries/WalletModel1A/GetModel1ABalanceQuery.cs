using Ecosystem.WalletService.Domain.DTOs.WalletModel1ADto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletModel1A;

public record GetModel1ABalanceQuery(int AffiliateId) : IRequest<BalanceInformationModel1ADto>;
