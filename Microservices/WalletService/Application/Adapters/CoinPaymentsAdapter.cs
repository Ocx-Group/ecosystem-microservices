using System.Globalization;
using System.Text.Json;
using Ecosystem.WalletService.Domain.Configuration;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.WalletService.Domain.Responses.BaseResponses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ecosystem.WalletService.Application.Adapters;

public class CoinPaymentsAdapter : CoinPaymentsBaseAdapter, ICoinPaymentsAdapter
{
    /// <summary>CoinPayments answers in snake_case while the DTOs use underscored Pascal names.</summary>
    private static readonly JsonSerializerOptions DeserializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ILogger<CoinPaymentsAdapter> _logger;

    public CoinPaymentsAdapter(
        HttpClient client,
        IOptions<ApplicationConfiguration> appSettings,
        ILogger<CoinPaymentsAdapter> logger)
        : base(client, appSettings, logger)
    {
        _logger = logger;
    }

    public bool VerifyIpnSignature(string rawBody, string? signature)
        => IsValidIpnSignature(rawBody, signature);

    public async Task<GetBasicInfoResponse?> GetProfile(string pbnTag)
    {
        var response = await CallApi(
            CoinPaymentsRoutes.GetProfileCommand,
            new SortedList<string, string> { { "pbntag", pbnTag } });

        return Deserialize<GetBasicInfoResponse>(response, nameof(GetProfile));
    }

    public async Task<GetDepositAddressResponse?> GetDepositAddress(string currency)
    {
        var response = await CallApi(
            CoinPaymentsRoutes.GetDepositAddressCommand,
            new SortedList<string, string> { { "currency", currency } });

        return Deserialize<GetDepositAddressResponse>(response, nameof(GetDepositAddress));
    }

    public async Task<GetCoinBalancesResponse?> GetCoinBalances(bool includeZeroBalances)
    {
        var parms = new SortedList<string, string>();
        if (includeZeroBalances)
            parms.Add("all", "1");

        var response = await CallApi(CoinPaymentsRoutes.GetBalancesCommand, parms);
        return Deserialize<GetCoinBalancesResponse>(response, nameof(GetCoinBalances));
    }

    public async Task<CreateConPaymentsTransactionResponse?> CreatePayment(ConPaymentRequest request)
    {
        var parms = new SortedList<string, string>
        {
            { "amount", request.Amount.ToString(CultureInfo.InvariantCulture) },
            { "currency1", request.Currency1 },
            { "currency2", request.Currency2 },
            { "buyer_email", request.BuyerEmail },
            { "address", request.Address }
        };

        AddOptional(parms, "buyer_name", request.BuyerName);
        AddOptional(parms, "item_name", request.ItemName);
        AddOptional(parms, "item_number", request.ItemNumber);
        AddOptional(parms, "invoice", request.Invoice);
        AddOptional(parms, "custom", request.Custom);
        AddOptional(parms, "ipn_url", request.IpnUrl);
        AddOptional(parms, "success_url", request.SuccessUrl);
        AddOptional(parms, "cancel_url", request.CancelUrl);

        var response = await CallApi(CoinPaymentsRoutes.CreateTransactionCommand, parms);
        return Deserialize<CreateConPaymentsTransactionResponse>(response, nameof(CreatePayment));
    }

    public async Task<GetTransactionInfoResponse?> GetTransactionInfo(string txnId, bool full)
    {
        var parms = new SortedList<string, string> { { "txid", txnId } };
        if (full)
            parms.Add("full", "1");

        var response = await CallApi(CoinPaymentsRoutes.GetTransactionInfoCommand, parms);
        return Deserialize<GetTransactionInfoResponse>(response, nameof(GetTransactionInfo));
    }

    public async Task<CoinPaymentWithdrawalResponse?> CreateMassWithdrawal(
        IEnumerable<CoinPaymentMassWithdrawalRequest> requests)
    {
        var parms = new SortedList<string, string>();
        var index = 0;

        // Keys are wd1..wdN in enumeration order; callers correlate results by that same order.
        foreach (var withdrawal in requests)
        {
            var prefix = $"wd[wd{++index}]";

            parms.Add($"{prefix}[amount]", withdrawal.Amount.ToString(CultureInfo.InvariantCulture));
            parms.Add($"{prefix}[address]", withdrawal.Address);
            parms.Add($"{prefix}[currency]", withdrawal.Currency);
        }

        if (index == 0)
        {
            _logger.LogWarning("CreateMassWithdrawal was called without any withdrawals");
            return null;
        }

        var response = await CallApi(CoinPaymentsRoutes.CreateMassWithdrawalCommand, parms);
        return Deserialize<CoinPaymentWithdrawalResponse>(response, nameof(CreateMassWithdrawal));
    }

    private static void AddOptional(SortedList<string, string> parms, string key, string? value)
    {
        if (value is not null)
            parms.Add(key, value);
    }

    private T? Deserialize<T>(IRestResponse response, string operation) where T : class
    {
        if (!response.IsSuccessful)
        {
            _logger.LogWarning(
                "CoinPayments {Operation} failed with HTTP {StatusCode}: {Reason}",
                operation, response.StatusCode, response.StatusDescription);
            return null;
        }

        if (string.IsNullOrWhiteSpace(response.Content))
        {
            _logger.LogWarning("CoinPayments {Operation} returned no content", operation);
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(response.Content, DeserializerOptions);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Could not deserialize the CoinPayments {Operation} response", operation);
            return null;
        }
    }
}
