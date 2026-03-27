using Ecosystem.WalletService.Domain.Responses.BaseResponses;

namespace Ecosystem.WalletService.Application.Adapters;

public interface IConfigurationAdapter
{
    Task<IRestResponse> GetMatrixConfiguration(long brandId, int matrixType);
    Task<IRestResponse> GetAllMatrixConfigurations(long brandId);
}
