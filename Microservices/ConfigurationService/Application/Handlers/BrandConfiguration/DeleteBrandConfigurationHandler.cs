using Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.BrandConfiguration;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public class DeleteBrandConfigurationHandler
    : IRequestHandler<DeleteBrandConfigurationCommand, BrandConfigurationDto?>
{
    private readonly IBrandConfigurationRepository _repository;
    private readonly IBrandConfigurationProvider _brandConfigProvider;
    private readonly ILogger<DeleteBrandConfigurationHandler> _logger;

    public DeleteBrandConfigurationHandler(
        IBrandConfigurationRepository repository,
        IBrandConfigurationProvider brandConfigProvider,
        ILogger<DeleteBrandConfigurationHandler> logger)
    {
        _repository = repository;
        _brandConfigProvider = brandConfigProvider;
        _logger = logger;
    }

    public async Task<BrandConfigurationDto?> Handle(
        DeleteBrandConfigurationCommand request,
        CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(request.BrandId);
        if (deleted is null) return null;

        await _brandConfigProvider.InvalidateCacheAsync(request.BrandId);

        _logger.LogInformation("Brand configuration deleted for BrandId {BrandId}", request.BrandId);

        return GetBrandConfigurationByBrandIdHandler.MapToDto(deleted);
    }
}
