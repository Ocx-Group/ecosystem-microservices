using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.Grpc.Configuration;

namespace Ecosystem.AccountService.Application.Adapters;

public class GrpcConfigurationServiceAdapter : IConfigurationServiceAdapter
{
    private readonly ConfigurationGrpc.ConfigurationGrpcClient _client;

    public GrpcConfigurationServiceAdapter(ConfigurationGrpc.ConfigurationGrpcClient client)
    {
        _client = client;
    }

    public async Task<MatrixConfigurationResult?> GetMatrixConfigurationAsync(long brandId, int matrixType)
    {
        var response = await _client.GetMatrixConfigurationAsync(
            new GetMatrixConfigurationRequest
            {
                BrandId = brandId,
                MatrixType = matrixType
            });

        if (!response.Success || response.Configuration is null)
            return null;

        return new MatrixConfigurationResult
        {
            MatrixName = response.Configuration.MatrixName
        };
    }

    public async Task<BrandConfigurationDto?> GetBrandConfigurationAsync(
        long brandId,
        CancellationToken cancellationToken = default)
    {
        var response = await _client.GetBrandConfigurationAsync(
            new GetBrandConfigurationRequest { BrandId = brandId },
            deadline: DateTime.UtcNow.AddSeconds(10),
            cancellationToken: cancellationToken);

        if (!response.Success || response.Configuration is null)
            return null;

        var source = response.Configuration;
        return new BrandConfigurationDto
        {
            BrandId = source.BrandId,
            Name = source.Name,
            AdminUserName = source.AdminUserName,
            SenderName = source.SenderName,
            SenderEmail = source.SenderEmail,
            EmailTemplateFolder = source.EmailTemplateFolder,
            ClientUrl = source.ClientUrl,
            CommissionEnabled = source.CommissionEnabled,
            CommissionLevels = source.CommissionLevels.Select(Convert.ToDecimal).ToArray(),
            BonusPercentage = Convert.ToDecimal(source.BonusPercentage),
            PdfTemplateName = source.PdfTemplateName,
            CompanyName = source.CompanyName,
            CompanyIdentifier = source.HasCompanyIdentifier ? source.CompanyIdentifier : null,
            SupportEmail = source.SupportEmail,
            SupportPhone = source.HasSupportPhone ? source.SupportPhone : null,
            DocumentType = source.HasDocumentType ? source.DocumentType : null,
            LogoUrl = source.HasLogoUrl ? source.LogoUrl : null,
            PrimaryColor = source.PrimaryColor,
            SecondaryColor = source.SecondaryColor,
            BackgroundColor = source.BackgroundColor,
            DefaultFatherAffiliateId = source.HasDefaultFatherAffiliateId
                ? source.DefaultFatherAffiliateId
                : null,
            ActivateOnRegistration = source.ActivateOnRegistration,
            DefaultPaymentGroupId = source.HasDefaultPaymentGroupId
                ? source.DefaultPaymentGroupId
                : null,
            TradingAcademyPaymentGroupId = source.HasTradingAcademyPaymentGroupId
                ? source.TradingAcademyPaymentGroupId
                : null,
            WithdrawalValidationType = source.WithdrawalValidationType,
            WithdrawalTimeZone = source.HasWithdrawalTimeZone
                ? source.WithdrawalTimeZone
                : null,
            WithdrawalStartHour = source.HasWithdrawalStartHour
                ? source.WithdrawalStartHour
                : null,
            WithdrawalEndHour = source.HasWithdrawalEndHour
                ? source.WithdrawalEndHour
                : null,
            WithdrawalCapNoDirects = source.HasWithdrawalCapNoDirects
                ? Convert.ToDecimal(source.WithdrawalCapNoDirects)
                : null,
            Requires10PercentPurchaseRule = source.Requires10PercentPurchaseRule,
            PoolValidationRequired = source.PoolValidationRequired,
            ConPaymentEnabled = source.ConPaymentEnabled,
            ConPaymentAddress = source.HasConPaymentAddress ? source.ConPaymentAddress : null,
            BlockchainNetworkId = source.HasBlockchainNetworkId
                ? source.BlockchainNetworkId
                : null,
            IsActive = source.IsActive
        };
    }
}
