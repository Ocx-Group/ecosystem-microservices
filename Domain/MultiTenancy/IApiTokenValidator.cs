namespace Ecosystem.Domain.Core.MultiTenancy;

/// <summary>
/// Validates API client tokens (Authorization header).
/// Each microservice implements this against its own api_clients table.
/// </summary>
public interface IApiTokenValidator
{
    Task<bool> ValidateAsync(string token);
}
