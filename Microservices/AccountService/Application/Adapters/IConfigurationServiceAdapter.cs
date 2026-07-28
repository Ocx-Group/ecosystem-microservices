using Ecosystem.Domain.Core.BrandConfiguration;

namespace Ecosystem.AccountService.Application.Adapters;

public interface IConfigurationServiceAdapter
{
    Task<MatrixConfigurationResult?> GetMatrixConfigurationAsync(long brandId, int matrixType);
    Task<BrandConfigurationDto?> GetBrandConfigurationAsync(
        long brandId,
        CancellationToken cancellationToken = default);
}

public class MatrixConfigurationResult
{
    public string MatrixName { get; set; } = string.Empty;
}
