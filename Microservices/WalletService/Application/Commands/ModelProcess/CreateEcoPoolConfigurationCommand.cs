using Ecosystem.WalletService.Domain.DTOs.ProcessGradingDto;
using Ecosystem.WalletService.Domain.Requests.EcoPoolConfigurationRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.ModelProcess;

public record CreateEcoPoolConfigurationCommand : IRequest<ModelConfigurationDto?>
{
    public int? Id { get; init; }
    public decimal CompanyPercentage { get; init; }
    public decimal CompanyPercentageLevels { get; init; }
    public decimal EcoPoolPercentage { get; init; }
    public decimal MaxGainLimit { get; init; }
    public DateTime DateInit { get; init; }
    public DateTime DateEnd { get; init; }
    public int Case { get; init; }
    public int? Processed { get; init; }
    public int? Totals { get; init; }
    public ICollection<LevelEcoPoolRequest> Levels { get; init; } = new List<LevelEcoPoolRequest>();
}
