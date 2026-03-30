using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Services;
using Ecosystem.WalletService.Domain.ValueObjects;

namespace Ecosystem.WalletService.Application.Services;

public class DebitTransactionBuilder : IDebitTransactionBuilder
{
    private readonly IBrandConfigurationProvider _brandConfigProvider;

    private int _affiliateId;
    private string _affiliateUserName = string.Empty;
    private string _concept = string.Empty;
    private string _conceptType = string.Empty;
    private decimal _debit;
    private decimal _points;
    private decimal _commissionable;
    private short _origin;
    private string _paymentMethod = string.Empty;
    private List<InvoiceDetailsTransactionRequest> _invoiceDetails = new();
    private string? _bank;
    private string? _receiptNumber;
    private bool _type;
    private bool? _includeInCommissionCalculation;

    public DebitTransactionBuilder(IBrandConfigurationProvider brandConfigProvider)
    {
        _brandConfigProvider = brandConfigProvider;
    }

    public IDebitTransactionBuilder ForAffiliate(int affiliateId, string affiliateUserName)
    {
        _affiliateId = affiliateId;
        _affiliateUserName = affiliateUserName;
        return this;
    }

    public IDebitTransactionBuilder WithConcept(string concept, string conceptType)
    {
        _concept = concept;
        _conceptType = conceptType;
        return this;
    }

    public IDebitTransactionBuilder WithAmounts(PaymentCalculation calculation)
    {
        _debit = calculation.Debit;
        _points = calculation.Points;
        _commissionable = calculation.Commissionable;
        _origin = calculation.Origin;
        return this;
    }

    public IDebitTransactionBuilder WithPaymentMethod(string paymentMethod)
    {
        _paymentMethod = paymentMethod;
        return this;
    }

    public IDebitTransactionBuilder WithInvoiceDetails(List<InvoiceDetailsTransactionRequest> invoiceDetails)
    {
        _invoiceDetails = invoiceDetails;
        return this;
    }

    public IDebitTransactionBuilder WithReceiptInfo(string? bank, string? receiptNumber, bool type)
    {
        _bank = bank;
        _receiptNumber = receiptNumber;
        _type = type;
        return this;
    }

    public IDebitTransactionBuilder WithCommissionCalculation(bool? includeInCommissionCalculation)
    {
        _includeInCommissionCalculation = includeInCommissionCalculation;
        return this;
    }

    public async Task<DebitTransactionRequest> BuildAsync(long brandId)
    {
        var brandConfig = await _brandConfigProvider.GetByBrandIdAsync(brandId);
        var adminUserName = brandConfig?.AdminUserName ?? string.Empty;

        var request = new DebitTransactionRequest
        {
            AffiliateId = _affiliateId,
            UserId = _affiliateId,
            Concept = _concept,
            Points = _points,
            Commissionable = _commissionable,
            PaymentMethod = _paymentMethod,
            Origin = _origin,
            Debit = _debit,
            AffiliateUserName = _affiliateUserName,
            AdminUserName = adminUserName,
            ConceptType = _conceptType,
            BrandId = brandId,
            Bank = _bank,
            ReceiptNumber = _receiptNumber,
            Type = _type,
            IncludeInCommissionCalculation = _includeInCommissionCalculation,
            invoices = _invoiceDetails
        };

        Reset();
        return request;
    }

    public IDebitTransactionBuilder Reset()
    {
        _affiliateId = 0;
        _affiliateUserName = string.Empty;
        _concept = string.Empty;
        _conceptType = string.Empty;
        _debit = 0;
        _points = 0;
        _commissionable = 0;
        _origin = 0;
        _paymentMethod = string.Empty;
        _invoiceDetails = new List<InvoiceDetailsTransactionRequest>();
        _bank = null;
        _receiptNumber = null;
        _type = false;
        _includeInCommissionCalculation = null;
        return this;
    }
}
