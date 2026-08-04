using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ecosystem.WalletService.Domain.Configuration;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Responses.BaseResponses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ecosystem.WalletService.Application.Adapters;

public class CoinPayTokenProvider : ICoinPayTokenProvider
{
    /// <summary>Safety margin so a token is never used in the seconds before it expires.</summary>
    private static readonly TimeSpan ExpirySkew = TimeSpan.FromSeconds(30);

    /// <summary>Used when the issued token carries no readable "exp" claim.</summary>
    private static readonly TimeSpan FallbackLifetime = TimeSpan.FromMinutes(5);

    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApplicationConfiguration _appSettings;
    private readonly ILogger<CoinPayTokenProvider> _logger;

    private string? _token;
    private DateTime _expiresAtUtc = DateTime.MinValue;

    public CoinPayTokenProvider(
        IHttpClientFactory httpClientFactory,
        IOptions<ApplicationConfiguration> appSettings,
        ILogger<CoinPayTokenProvider> logger)
    {
        _httpClientFactory = httpClientFactory;
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public void Invalidate()
    {
        _token = null;
        _expiresAtUtc = DateTime.MinValue;
    }

    public async Task<string?> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        if (IsCurrent())
            return _token;

        await _lock.WaitAsync(cancellationToken);
        try
        {
            // Another caller may have refreshed the token while we waited on the lock.
            if (IsCurrent())
                return _token;

            var token = await Authenticate(cancellationToken);
            if (token is null)
                return null;

            _token = token;
            _expiresAtUtc = ResolveExpiry(token);

            return _token;
        }
        finally
        {
            _lock.Release();
        }
    }

    private bool IsCurrent() => _token is not null && DateTime.UtcNow < _expiresAtUtc;

    private async Task<string?> Authenticate(CancellationToken cancellationToken)
    {
        var initialToken = _appSettings.CoinPay?.InitialToken;
        var secretKey = _appSettings.CoinPay?.SecretKey;

        // Checked for emptiness, not just null: the versioned appsettings.json ships these
        // blank, and signing with a blank secret makes CoinPay reject every call — which
        // then surfaces as a misleading "not found" instead of a configuration error.
        if (string.IsNullOrWhiteSpace(initialToken) || string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException(
                "AppSettings:CoinPay:SecretKey and AppSettings:CoinPay:InitialToken must be configured.");
        }

        var idRequest = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

        var checksumHash = SHA256.HashData(Encoding.UTF8.GetBytes(idRequest + secretKey));
        var checksum = Convert.ToHexString(checksumHash).ToLowerInvariant();

        var body = new
        {
            IdRequest = idRequest,
            Token = initialToken,
            Checksum = checksum
        };

        using var client = _httpClientFactory.CreateClient(CoinPayConstants.HttpClientName);
        using var content = new StringContent(body.ToJsonString(), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(CoinPayRoutes.CreateTokenRoute, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("CoinPay authentication failed with HTTP status {StatusCode}", response.StatusCode);
            return null;
        }

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = payload.ToJsonObject<CoinPayResponse>();

        if (result?.StatusCode != Constants.SuccessStatusCode || string.IsNullOrEmpty(result.Data?.Token))
        {
            _logger.LogError("CoinPay authentication rejected with status code {StatusCode}", result?.StatusCode);
            return null;
        }

        return result.Data.Token;
    }

    /// <summary>
    /// Reads the "exp" claim straight off the JWT payload so the cache follows the
    /// issuer's own lifetime. Falls back to a short window when it cannot be read.
    /// </summary>
    private static DateTime ResolveExpiry(string token)
    {
        var fallback = DateTime.UtcNow.Add(FallbackLifetime);

        var segments = token.Split('.');
        if (segments.Length < 2)
            return fallback;

        try
        {
            var payload = segments[1].Replace('-', '+').Replace('_', '/');
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

            using var document = JsonDocument.Parse(Convert.FromBase64String(payload));
            if (!document.RootElement.TryGetProperty("exp", out var exp) || !exp.TryGetInt64(out var seconds))
                return fallback;

            var expiresAt = DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime - ExpirySkew;

            // CoinPay's seed token is known to be already expired; the provider only
            // validates the checksum, so an expiry in the past must not poison the cache.
            return expiresAt <= DateTime.UtcNow ? fallback : expiresAt;
        }
        catch
        {
            return fallback;
        }
    }
}
