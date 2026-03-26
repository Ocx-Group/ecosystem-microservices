namespace Ecosystem.ConfigurationService.Domain.Constants;

public static class ConfigurationEnums
{
    public enum MatrixConfigurations
    {
        UniLevelMatrix,
        ForceMatrix,
        AffiliatesFrontNum,
        SoftwareMillenniumFrontNum,
        BinaryMatrix
    }

    public enum ProductsConfigurations
    {
        ActivatePublicShop,
        ActivateShippingSystem,
        ActivatePassivePaymentsModule,
        CurrencySymbol,
        SymbolCommissionableValue,
        SymbolPointsQualify,
        BinaryPointsSymbol,
        NewProductLabel
    }

    public enum CompensationPlansConfigurations
    {
        AutomaticActivation,
        AutomaticQualification,
        AutomaticIncentiveCalculation,
        AutomaticCommissionCalculation
    }

    public enum WithdrawalsWalletConfigurations
    {
        MinimumAmount,
        MaximumAmount,
        CommissionAmount,
        ActivateInvoiceCancellation
    }

    public enum AdditionalParametersWalletConfigurations
    {
        MinutesValidityCode,
        ConceptWalletWithdrawal,
        ActivateConfirmationMails
    }

    public enum GeneralConfigurations
    {
        PaymentModelCutoffDate,
        IsUnderMaintenance
    }
}
