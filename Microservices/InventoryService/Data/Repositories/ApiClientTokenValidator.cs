using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;

namespace Ecosystem.InventoryService.Data.Repositories;

public class ApiClientTokenValidator : IApiTokenValidator
{
    private readonly IApiClientRepository _apiClientRepository;

    public ApiClientTokenValidator(IApiClientRepository apiClientRepository)
        => _apiClientRepository = apiClientRepository;

    public Task<bool> ValidateAsync(string token)
        => _apiClientRepository.ValidateApiClient(token);
}
