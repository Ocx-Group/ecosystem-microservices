namespace Ecosystem.NotificationService.Domain.Interfaces;

public interface IApiClientRepository
{
    Task<bool> ValidateApiClientAsync(string token);
}
