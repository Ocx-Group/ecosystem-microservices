using AutoMapper;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.MatrixConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.MatrixConfiguration;

public class GetMatrixConfigurationByTypeHandler : IRequestHandler<GetMatrixConfigurationByTypeQuery, MatrixConfigDto?>
{
    private readonly IMatrixConfigurationRepository _matrixConfigurationRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetMatrixConfigurationByTypeHandler> _logger;

    public GetMatrixConfigurationByTypeHandler(
        IMatrixConfigurationRepository matrixConfigurationRepository,
        IMapper mapper,
        ILogger<GetMatrixConfigurationByTypeHandler> logger)
    {
        _matrixConfigurationRepository = matrixConfigurationRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MatrixConfigDto?> Handle(GetMatrixConfigurationByTypeQuery request, CancellationToken cancellationToken)
    {
        var matrixConfig = await _matrixConfigurationRepository.GetMatrixConfigurationByType(request.MatrixType);
        if (matrixConfig is null) return null;

        return _mapper.Map<MatrixConfigDto>(matrixConfig);
    }
}
