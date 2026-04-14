using AutoMapper;
using Ecosystem.Grpc.Configuration;
using Ecosystem.WalletService.Domain.Responses;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Adapters;

public class GrpcConfigurationAdapter : IConfigurationAdapter
{
    private readonly ConfigurationGrpc.ConfigurationGrpcClient _client;
    private readonly IMapper _mapper;
    private readonly ILogger<GrpcConfigurationAdapter> _logger;

    public GrpcConfigurationAdapter(
        ConfigurationGrpc.ConfigurationGrpcClient client,
        IMapper mapper,
        ILogger<GrpcConfigurationAdapter> logger)
    {
        _client = client;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MatrixConfiguration?> GetMatrixConfiguration(long brandId, int matrixType)
    {
        try
        {
            var response = await _client.GetMatrixConfigurationAsync(new GetMatrixConfigurationRequest
            {
                BrandId = brandId,
                MatrixType = matrixType
            });
            if (!response.Success || response.Configuration is null) return null;
            return _mapper.Map<MatrixConfiguration>(response.Configuration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in GetMatrixConfiguration for type {MatrixType}", matrixType);
            return null;
        }
    }

    public async Task<List<MatrixConfiguration>?> GetAllMatrixConfigurations(long brandId)
    {
        try
        {
            var response = await _client.GetAllMatrixConfigurationsAsync(new GetAllMatrixConfigurationsRequest
            {
                BrandId = brandId
            });
            if (!response.Success) return null;
            return response.Configurations.Select(_mapper.Map<MatrixConfiguration>).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in GetAllMatrixConfigurations");
            return null;
        }
    }
}
