using Ecosystem.Grpc.Configuration;

namespace Ecosystem.AccountService.Application.Adapters;

public class GrpcConfigurationServiceAdapter : IConfigurationServiceAdapter
{
    private readonly ConfigurationGrpc.ConfigurationGrpcClient _client;

    public GrpcConfigurationServiceAdapter(ConfigurationGrpc.ConfigurationGrpcClient client)
    {
        _client = client;
    }

    public async Task<MatrixConfigurationResult?> GetMatrixConfigurationAsync(long brandId, int matrixType)
    {
        var response = await _client.GetMatrixConfigurationAsync(
            new GetMatrixConfigurationRequest
            {
                BrandId = brandId,
                MatrixType = matrixType
            });

        if (!response.Success || response.Configuration is null)
            return null;

        return new MatrixConfigurationResult
        {
            MatrixName = response.Configuration.MatrixName
        };
    }
}
