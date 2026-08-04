using System.Text.Json;
using Ecosystem.WalletService.Domain.Configuration;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.WalletService.Domain.Responses.BaseResponses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ecosystem.WalletService.Application.Adapters;

public class CoinPayAdapter : CoinPayBaseAdapter, ICoinPayAdapter
{
    /// <summary>CoinPay answers in camelCase while the DTOs are PascalCase.</summary>
    private static readonly JsonSerializerOptions DeserializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ILogger<CoinPayAdapter> _logger;

    public CoinPayAdapter(
        HttpClient client,
        ICoinPayTokenProvider tokenProvider,
        IOptions<ApplicationConfiguration> appSettings,
        ILogger<CoinPayAdapter> logger)
        : base(client, tokenProvider, appSettings)
    {
        _logger = logger;
    }

    public Task<bool> VerifyTransactionSignature(SignatureParamsRequest request)
        => IsValidSignature(request);

    public async Task<CreateTransactionResponse?> CreateTransaction(PaymentRequest request)
    {
        var response = await Post(CoinPayRoutes.CreateTransactionRoute, request);
        return Deserialize<CreateTransactionResponse>(response, nameof(CreateTransaction));
    }

    public async Task<CreateChannelResponse?> CreateChannel(CreateChannelRequest request)
    {
        var response = await Post(CoinPayRoutes.CreateChannelRoute, request);
        return Deserialize<CreateChannelResponse>(response, nameof(CreateChannel));
    }

    public async Task<GetNetworkResponse?> GetNetworksByIdCurrency(int currencyId)
    {
        var response = await Get(string.Format(CoinPayRoutes.GetNetworksByIdCurrencyRoute, currencyId));
        return Deserialize<GetNetworkResponse>(response, nameof(GetNetworksByIdCurrency));
    }

    public async Task<CreateAddressResponse?> CreateAddress(CreateAddresRequest request)
    {
        var response = await Post(
            string.Format(CoinPayRoutes.CreateAddressRoute, Constants.CoinPayIdWallet), request);

        return Deserialize<CreateAddressResponse>(response, nameof(CreateAddress));
    }

    public async Task<GetTransactionByIdResponse?> GetTransactionById(int transactionId)
    {
        var response = await Get(string.Format(CoinPayRoutes.GetTransactionRoute, transactionId));
        return Deserialize<GetTransactionByIdResponse>(response, nameof(GetTransactionById));
    }

    public async Task<SendFundsResponse?> SendFunds(SendFundRequest request)
    {
        var response = await Post(CoinPayRoutes.SendFundsRoute, request);
        return Deserialize<SendFundsResponse>(response, nameof(SendFunds));
    }

    private T? Deserialize<T>(IRestResponse response, string operation) where T : class
    {
        if (string.IsNullOrWhiteSpace(response.Content))
        {
            _logger.LogWarning(
                "CoinPay {Operation} returned no content (HTTP {StatusCode})", operation, response.StatusCode);
            return null;
        }

        if (!response.IsSuccessful)
        {
            _logger.LogWarning(
                "CoinPay {Operation} failed with HTTP {StatusCode}: {Content}",
                operation, response.StatusCode, response.Content);
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(response.Content, DeserializerOptions);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Could not deserialize the CoinPay {Operation} response", operation);
            return null;
        }
    }
}
