using AutoMapper;
using Ecosystem.WalletService.Application.Commands.ModelProcess;
using Ecosystem.WalletService.Domain.DTOs.ProcessGradingDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.ModelProcess;

public class CreateEcoPoolConfigurationHandler : IRequestHandler<CreateEcoPoolConfigurationCommand, ModelConfigurationDto?>
{
    private readonly IEcoPoolConfigurationRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateEcoPoolConfigurationHandler> _logger;

    public CreateEcoPoolConfigurationHandler(
        IEcoPoolConfigurationRepository repository,
        IMapper mapper,
        ILogger<CreateEcoPoolConfigurationHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ModelConfigurationDto?> Handle(CreateEcoPoolConfigurationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating/updating EcoPool configuration");

        ModelConfiguration configuration;

        if (request.Id.HasValue && request.Id.Value > 0)
        {
            var existing = await _repository.GetConfiguration();
            if (existing == null)
                return null;

            existing.CompanyPercentage = request.CompanyPercentage;
            existing.CompanyPercentageLevels = request.CompanyPercentageLevels;
            existing.ModelPercentage = request.EcoPoolPercentage;
            existing.MaxGainLimit = request.MaxGainLimit;
            existing.DateInit = request.DateInit;
            existing.DateEnd = request.DateEnd;
            existing.Case = request.Case;
            existing.Processed = request.Processed;
            existing.Totals = request.Totals;
            existing.UpdatedAt = DateTime.UtcNow;

            configuration = await _repository.UpdateConfiguration(existing);

            await _repository.DeleteAllLevelsConfiguration(configuration.Id);
        }
        else
        {
            var newConfig = new ModelConfiguration
            {
                CompanyPercentage = request.CompanyPercentage,
                CompanyPercentageLevels = request.CompanyPercentageLevels,
                ModelPercentage = request.EcoPoolPercentage,
                MaxGainLimit = request.MaxGainLimit,
                DateInit = request.DateInit,
                DateEnd = request.DateEnd,
                Case = request.Case,
                Processed = request.Processed,
                Totals = request.Totals,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            configuration = await _repository.CreateConfiguration(newConfig);
        }

        if (request.Levels.Any())
        {
            var levels = request.Levels.Select(l => new ModelConfigurationLevel
            {
                Level = l.Level,
                Percentage = l.Percentage,
                EcopoolConfigurationId = configuration.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await _repository.CreateConfigurationLevels(levels);
        }

        return _mapper.Map<ModelConfigurationDto>(configuration);
    }
}
