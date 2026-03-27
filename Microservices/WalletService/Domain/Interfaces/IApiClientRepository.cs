namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IApiClientRepository
{
    Task<bool> ValidateApiClient(string token);
}