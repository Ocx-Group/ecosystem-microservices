using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.Configuration;

public record SetGeneralConfigurationCommand : IRequest<GeneralConfigurationDto>
{
    public DateTime PaymentModelCutoffDate { get; init; }
    public bool IsUnderMaintenance { get; init; }
}
