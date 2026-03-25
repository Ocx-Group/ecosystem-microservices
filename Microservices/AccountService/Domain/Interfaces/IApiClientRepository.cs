namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IApiClientRepository
{
    Task<bool> ValidateApiClient(string token);
}
