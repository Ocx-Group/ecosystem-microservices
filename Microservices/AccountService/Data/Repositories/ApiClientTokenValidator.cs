using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;

namespace Ecosystem.AccountService.Data.Repositories;

/// <summary>
/// Adapts IApiClientRepository to the shared IApiTokenValidator contract.
/// </summary>
public class ApiClientTokenValidator : IApiTokenValidator
{
    private readonly IApiClientRepository _apiClientRepository;

    public ApiClientTokenValidator(IApiClientRepository apiClientRepository)
        => _apiClientRepository = apiClientRepository;

    public Task<bool> ValidateAsync(string token)
        => _apiClientRepository.ValidateApiClient(token);
}
