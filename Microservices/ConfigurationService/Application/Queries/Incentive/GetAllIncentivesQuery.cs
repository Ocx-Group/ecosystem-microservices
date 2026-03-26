using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.Incentive;

public record GetAllIncentivesQuery : IRequest<ICollection<IncentiveDto>>;
