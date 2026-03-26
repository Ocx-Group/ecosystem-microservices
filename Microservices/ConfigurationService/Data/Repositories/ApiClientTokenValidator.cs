using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class ApiClientTokenValidator : IApiTokenValidator
{
    private readonly IApiClientRepository _apiClientRepository;

    public ApiClientTokenValidator(IApiClientRepository apiClientRepository)
        => _apiClientRepository = apiClientRepository;

    public Task<bool> ValidateAsync(string token)
        => _apiClientRepository.ValidateApiClient(token);
}
