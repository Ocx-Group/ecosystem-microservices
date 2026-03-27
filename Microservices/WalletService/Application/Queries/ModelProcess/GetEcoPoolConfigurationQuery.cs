using Ecosystem.WalletService.Domain.DTOs.ProcessGradingDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.ModelProcess;

public record GetEcoPoolConfigurationQuery : IRequest<ModelConfigurationDto?>;
