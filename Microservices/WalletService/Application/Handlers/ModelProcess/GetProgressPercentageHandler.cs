using AutoMapper;
using Ecosystem.WalletService.Application.Queries.ModelProcess;
using Ecosystem.WalletService.Domain.DTOs.ProcessGradingDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.ModelProcess;

public class GetProgressPercentageHandler : IRequestHandler<GetProgressPercentageQuery, ModelConfigurationDto?>
{
    private readonly IEcoPoolConfigurationRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProgressPercentageHandler> _logger;

    public GetProgressPercentageHandler(
        IEcoPoolConfigurationRepository repository,
        IMapper mapper,
        ILogger<GetProgressPercentageHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ModelConfigurationDto?> Handle(GetProgressPercentageQuery request, CancellationToken cancellationToken)
    {
        var configuration = await _repository.GetProgressPercentage(request.ConfigurationId);
        return configuration == null ? null : _mapper.Map<ModelConfigurationDto>(configuration);
    }
}
