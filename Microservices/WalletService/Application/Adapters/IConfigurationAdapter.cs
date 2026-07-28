using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Application.Adapters;

public interface IConfigurationAdapter
{
    Task<MatrixConfiguration?> GetMatrixConfiguration(long brandId, int matrixType);
    Task<List<MatrixConfiguration>?> GetAllMatrixConfigurations(long brandId);
    Task<BrandConfigurationDto?> GetBrandConfiguration(
        long brandId,
        CancellationToken cancellationToken = default);
}
