namespace Ecosystem.AccountService.Application.Adapters;

// TODO: Replace with gRPC or RabbitMQ call to ConfigurationService
public class ConfigurationServiceAdapter : IConfigurationServiceAdapter
{
    public Task<MatrixConfigurationResult?> GetMatrixConfigurationAsync(long brandId, int matrixType)
    {
        return Task.FromResult<MatrixConfigurationResult?>(new MatrixConfigurationResult
        {
            MatrixName = $"Matrix {matrixType}"
        });
    }
}
