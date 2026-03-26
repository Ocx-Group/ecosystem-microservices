namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IApiClientRepository
{
    Task<bool> ValidateApiClient(string token);
}
