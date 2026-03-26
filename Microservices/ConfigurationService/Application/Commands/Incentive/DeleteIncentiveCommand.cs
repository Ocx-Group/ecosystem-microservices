using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.Incentive;

public record DeleteIncentiveCommand(int Id) : IRequest<IncentiveDto?>;
