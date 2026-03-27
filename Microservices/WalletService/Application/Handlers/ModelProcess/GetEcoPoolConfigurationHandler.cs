using AutoMapper;
using Ecosystem.WalletService.Application.Queries.ModelProcess;
using Ecosystem.WalletService.Domain.DTOs.ProcessGradingDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.ModelProcess;

public class GetEcoPoolConfigurationHandler : IRequestHandler<GetEcoPoolConfigurationQuery, ModelConfigurationDto?>
{
    private readonly IEcoPoolConfigurationRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetEcoPoolConfigurationHandler> _logger;

    public GetEcoPoolConfigurationHandler(
        IEcoPoolConfigurationRepository repository,
        IMapper mapper,
        ILogger<GetEcoPoolConfigurationHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ModelConfigurationDto?> Handle(GetEcoPoolConfigurationQuery request, CancellationToken cancellationToken)
    {
        var configuration = await _repository.GetConfiguration();
        return configuration == null ? null : _mapper.Map<ModelConfigurationDto>(configuration);
    }
}
