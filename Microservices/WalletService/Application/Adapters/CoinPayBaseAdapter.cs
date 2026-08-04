using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Ecosystem.WalletService.Domain.Configuration;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Responses.BaseResponses;
using Microsoft.Extensions.Options;

namespace Ecosystem.WalletService.Application.Adapters;

/// <summary>
/// Transport layer for CoinPay: bearer token handling, request signing and the
/// GET/POST primitives. Business rules belong in the MediatR handlers.
/// </summary>
public abstract class CoinPayBaseAdapter
{
    private readonly HttpClient _client;
    private readonly ICoinPayTokenProvider _tokenProvider;
    private readonly string _sharedKey;

    protected CoinPayBaseAdapter(
        HttpClient client,
        ICoinPayTokenProvider tokenProvider,
        IOptions<ApplicationConfiguration> appSettings)
    {
        _client = client;
        _tokenProvider = tokenProvider;
        _sharedKey = appSettings.Value.CoinPay?.SecretKey
                     ?? throw new InvalidOperationException("AppSettings:CoinPay:SecretKey is not configured.");
    }

    protected Task<bool> IsValidSignature(SignatureParamsRequest request)
    {
        if (string.IsNullOrEmpty(request.IncomingSignature))
            return Task.FromResult(false);

        var expected = GenerateSignature(request);

        // Fixed-time comparison so a caller cannot probe the signature byte by byte.
        var isValid = CryptographicOperations.FixedTimeEquals(
            Encoding.ASCII.GetBytes(expected),
            Encoding.ASCII.GetBytes(request.IncomingSignature));

        return Task.FromResult(isValid);
    }

    private string GenerateSignature(SignatureParamsRequest request)
    {
        var tmpInfo = $"{request.IdUser}{request.IdTransaction}{request.DynamicKey}";
        using var hmac = new HMACSHA512(Encoding.ASCII.GetBytes(_sharedKey));
        var hashBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(tmpInfo));
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }

    protected async Task<IRestResponse> Get(string path, Dictionary<string, string>? queryParams = null)
    {
        if (queryParams is { Count: > 0 })
            path = $"{path}?{string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"))}";

        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        return await Send(request);
    }

    protected async Task<IRestResponse> Post(string path, object data)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, path)
        {
            Content = new StringContent(data.ToJsonString(), Encoding.UTF8, "application/json")
        };

        return await Send(request);
    }

    /// <summary>
    /// Authorises and sends the request. The token travels on the message itself —
    /// never on <see cref="HttpClient.DefaultRequestHeaders"/> — so concurrent calls
    /// sharing the client cannot overwrite each other's credentials.
    /// </summary>
    private async Task<IRestResponse> Send(HttpRequestMessage request)
    {
        var token = await _tokenProvider.GetTokenAsync();
        if (token is null)
        {
            // Fully qualified: this namespace declares a RestResponse of its own.
            return new Domain.Responses.BaseResponses.RestResponse
            {
                Content = null,
                StatusCode = HttpStatusCode.Unauthorized,
                StatusDescription = "Could not obtain a CoinPay access token."
            };
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _client.SendAsync(request);

        // A rejected token is usually a stale cache entry; drop it so the next call re-authenticates.
        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            _tokenProvider.Invalidate();

        return new Domain.Responses.BaseResponses.RestResponse
        {
            Content = await response.Content.ReadAsStringAsync(),
            StatusCode = response.StatusCode,
            StatusDescription = response.ReasonPhrase
        };
    }
}
