using AutoMapper;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.MatrixConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.MatrixConfiguration;

public class GetAllMatrixConfigurationsHandler : IRequestHandler<GetAllMatrixConfigurationsQuery, IEnumerable<MatrixConfigDto>?>
{
    private readonly IMatrixConfigurationRepository _matrixConfigurationRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllMatrixConfigurationsHandler> _logger;

    public GetAllMatrixConfigurationsHandler(
        IMatrixConfigurationRepository matrixConfigurationRepository,
        IMapper mapper,
        ILogger<GetAllMatrixConfigurationsHandler> logger)
    {
        _matrixConfigurationRepository = matrixConfigurationRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<MatrixConfigDto>?> Handle(GetAllMatrixConfigurationsQuery request, CancellationToken cancellationToken)
    {
        var matrixConfigs = await _matrixConfigurationRepository.GetAllMatrixConfigurations();
        if (matrixConfigs is null) return null;

        return _mapper.Map<IEnumerable<MatrixConfigDto>>(matrixConfigs);
    }
}
