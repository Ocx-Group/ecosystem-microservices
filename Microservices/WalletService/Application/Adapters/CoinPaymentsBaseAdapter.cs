using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Ecosystem.WalletService.Domain.Configuration;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Responses.BaseResponses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ecosystem.WalletService.Application.Adapters;

/// <summary>
/// Transport layer for coinpayments.net: form encoding, HMAC signing and the single POST
/// primitive the provider exposes. Business rules belong in the MediatR handlers.
/// </summary>
public abstract class CoinPaymentsBaseAdapter
{
    private static readonly Encoding PayloadEncoding = Encoding.UTF8;

    private readonly HttpClient _client;
    private readonly ILogger _logger;
    private readonly string _publicKey;
    private readonly string _privateKey;
    private readonly string? _ipnSecret;

    protected CoinPaymentsBaseAdapter(
        HttpClient client,
        IOptions<ApplicationConfiguration> appSettings,
        ILogger logger)
    {
        _client = client;
        _logger = logger;

        var settings = appSettings.Value.ConPayments;

        // Checked for emptiness, not just null: the versioned appsettings.json ships these
        // blank, and signing with a blank secret makes the provider answer with a generic
        // error that reads like a data problem instead of a configuration one.
        if (string.IsNullOrWhiteSpace(settings?.Key) || string.IsNullOrWhiteSpace(settings.Secret))
        {
            throw new InvalidOperationException(
                "AppSettings:ConPayments:Key and AppSettings:ConPayments:Secret must be configured.");
        }

        _publicKey = settings.Key;
        _privateKey = settings.Secret;
        _ipnSecret = settings.IpnSecret;
    }

    /// <summary>
    /// Signs and posts a command. The parameters travel in a <see cref="SortedList{TKey,TValue}"/>
    /// so the body is always built in the same order, which is what the digest is taken over.
    /// </summary>
    protected async Task<IRestResponse> CallApi(string cmd, SortedList<string, string>? parms = null)
    {
        parms ??= new SortedList<string, string>();

        parms["version"] = "1";
        parms["key"] = _publicKey;
        parms["cmd"] = cmd;
        parms["nonce"] = DateTime.UtcNow.Ticks.ToString();

        var postData = string.Join(
            "&", parms.Select(parm => $"{parm.Key}={Uri.EscapeDataString(parm.Value)}"));

        using var request = new HttpRequestMessage(HttpMethod.Post, CoinPaymentsRoutes.ApiPath)
        {
            // StringContent sends the string verbatim. FormUrlEncodedContent would re-encode it
            // and the digest below — taken over this exact string — would no longer match.
            Content = new StringContent(postData, PayloadEncoding, "application/x-www-form-urlencoded")
        };

        request.Headers.Add("HMAC", ComputeHmac(_privateKey, postData));
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {
            var response = await _client.SendAsync(request);

            return new Domain.Responses.BaseResponses.RestResponse
            {
                Content = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode,
                StatusDescription = response.ReasonPhrase
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Could not reach CoinPayments while running {Command}", cmd);

            return new Domain.Responses.BaseResponses.RestResponse
            {
                Content = null,
                StatusCode = HttpStatusCode.ServiceUnavailable,
                StatusDescription = ex.Message
            };
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "The CoinPayments call {Command} timed out", cmd);

            return new Domain.Responses.BaseResponses.RestResponse
            {
                Content = null,
                StatusCode = HttpStatusCode.GatewayTimeout,
                StatusDescription = ex.Message
            };
        }
    }

    /// <summary>
    /// Verifies the digest CoinPayments puts in the <c>Hmac</c> header against the raw request
    /// body. Callers must pass the body exactly as received: any re-serialisation invalidates it.
    /// </summary>
    protected bool IsValidIpnSignature(string rawBody, string? incomingSignature)
    {
        if (string.IsNullOrWhiteSpace(incomingSignature) || string.IsNullOrWhiteSpace(_ipnSecret))
            return false;

        // The provider sends lowercase hex while ComputeHmac produces uppercase, so both sides
        // are normalised before the fixed-time comparison.
        var expected = ComputeHmac(_ipnSecret, rawBody).ToLowerInvariant();

        return CryptographicOperations.FixedTimeEquals(
            PayloadEncoding.GetBytes(expected),
            PayloadEncoding.GetBytes(incomingSignature.Trim().ToLowerInvariant()));
    }

    private static string ComputeHmac(string key, string payload)
    {
        using var hmac = new HMACSHA512(PayloadEncoding.GetBytes(key));
        return Convert.ToHexString(hmac.ComputeHash(PayloadEncoding.GetBytes(payload)));
    }
}
