namespace Ecosystem.InventoryService.Domain.Interfaces;

public interface IApiClientRepository
{
    Task<bool> ValidateApiClient(string token);
}
