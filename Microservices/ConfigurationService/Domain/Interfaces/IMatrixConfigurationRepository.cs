using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IMatrixConfigurationRepository
{
    Task<MatrixConfiguration?> GetMatrixConfigurationByType(int matrixType);
    Task<IEnumerable<MatrixConfiguration?>> GetAllMatrixConfigurations();
}
