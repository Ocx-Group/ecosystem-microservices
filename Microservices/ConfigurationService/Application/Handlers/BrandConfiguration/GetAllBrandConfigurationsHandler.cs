using Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.BrandConfiguration;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public class GetAllBrandConfigurationsHandler
    : IRequestHandler<GetAllBrandConfigurationsQuery, IReadOnlyList<BrandConfigurationDto>>
{
    private readonly IBrandConfigurationRepository _repository;
    private readonly ILogger<GetAllBrandConfigurationsHandler> _logger;

    public GetAllBrandConfigurationsHandler(
        IBrandConfigurationRepository repository,
        ILogger<GetAllBrandConfigurationsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<BrandConfigurationDto>> Handle(
        GetAllBrandConfigurationsQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync();

        return entities
            .Select(GetBrandConfigurationByBrandIdHandler.MapToDto)
            .ToList()
            .AsReadOnly();
    }
}
