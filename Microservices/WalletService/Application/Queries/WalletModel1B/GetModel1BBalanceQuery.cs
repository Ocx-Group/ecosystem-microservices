using Ecosystem.WalletService.Domain.DTOs.WalletModel1BDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletModel1B;

public record GetModel1BBalanceQuery(int AffiliateId) : IRequest<BalanceInformationModel1BDto>;
