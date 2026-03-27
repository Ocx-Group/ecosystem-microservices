using Ecosystem.WalletService.Domain.DTOs.ProcessGradingDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.ModelProcess;

public record GetProgressPercentageQuery(int ConfigurationId) : IRequest<ModelConfigurationDto?>;
