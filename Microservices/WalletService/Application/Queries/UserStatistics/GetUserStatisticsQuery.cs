using Ecosystem.WalletService.Domain.DTOs.AffiliateInformation;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.UserStatistics;

public record GetUserStatisticsQuery(int UserId) : IRequest<Domain.DTOs.AffiliateInformation.UserStatistics>;
