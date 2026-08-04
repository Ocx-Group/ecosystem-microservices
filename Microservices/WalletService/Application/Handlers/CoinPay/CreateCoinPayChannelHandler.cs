using System.Text.Json;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

/// <summary>
/// Main collection flow: opens a dedicated deposit channel on CoinPay and records the
/// local transaction the webhook will later match on <see cref="Transaction.Reference"/>.
/// </summary>
public class CreateCoinPayChannelHandler : IRequestHandler<CreateCoinPayChannelCommand, CreateChannelResponse?>
{
    private readonly ICoinPayAdapter _coinPayAdapter;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CreateCoinPayChannelHandler> _logger;

    public CreateCoinPayChannelHandler(
        ICoinPayAdapter coinPayAdapter,
        ITransactionRepository transactionRepository,
        ITenantContext tenantContext,
        ILogger<CreateCoinPayChannelHandler> logger)
    {
        _coinPayAdapter = coinPayAdapter;
        _transactionRepository = transactionRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<CreateChannelResponse?> Handle(
        CreateCoinPayChannelCommand request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var externalIdentification = CommonExtensions.GenerateUniqueId(request.AffiliateId);

        var channelRequest = new CreateChannelRequest
        {
            IdCurrency = request.IdCurrency,
            IdNetwork = request.IdNetwork,
            IdExternalIdentification = externalIdentification,
            TagName = request.TagName
        };

        var channel = await _coinPayAdapter.CreateChannel(channelRequest);

        if (channel?.Data is null || channel.StatusCode != Constants.SuccessStatusCode)
        {
            _logger.LogWarning(
                "CoinPay channel creation failed for affiliate {AffiliateId} with status code {StatusCode}",
                request.AffiliateId, channel?.StatusCode);
            return null;
        }

        var now = DateTime.UtcNow;
        var idTransaction = channel.Data.Id.ToString();
        var products = JsonSerializer.Serialize(request.Products);

        // The reference is taken from the response, not from the value generated above:
        // the clients poll getTransactionByReference with channel.data.idExternalIdentification,
        // and that is also what CoinPay sends back on the webhook.
        var reference = channel.Data.IdExternalIdentification.ToString();

        var existing = await _transactionRepository.GetTransactionByIdTransaction(idTransaction, brandId);

        Transaction? persisted;

        if (existing is { Acredited: false })
        {
            existing.Amount = request.Amount;
            existing.Products = products;
            // Kept in step with the response the client is about to poll and render.
            existing.Reference = reference;
            existing.Address = channel.Data.Address;
            existing.UpdatedAt = now;

            persisted = await _transactionRepository.UpdateTransactionAsync(existing);
        }
        else
        {
            persisted = await _transactionRepository.CreateTransaction(new Transaction
            {
                IdTransaction = idTransaction,
                AffiliateId = request.AffiliateId,
                Amount = request.Amount,
                AmountReceived = Constants.EmptyValue,
                Products = products,
                Acredited = false,
                Status = Constants.EmptyValue,
                PaymentMethod = Constants.CoinPay,
                Reference = reference,
                Address = channel.Data.Address,
                CreatedAt = now,
                UpdatedAt = now,
                BrandId = brandId
            });
        }

        if (persisted is null)
        {
            _logger.LogError(
                "Could not record the CoinPay transaction for affiliate {AffiliateId}", request.AffiliateId);
            return null;
        }

        _logger.LogInformation(
            "CoinPay channel {IdTransaction} ready for affiliate {AffiliateId} with reference {Reference}",
            idTransaction, request.AffiliateId, reference);

        return channel;
    }
}
