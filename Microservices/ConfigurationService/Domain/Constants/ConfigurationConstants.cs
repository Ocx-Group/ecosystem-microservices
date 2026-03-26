namespace Ecosystem.ConfigurationService.Domain.Constants;

public static class ConfigurationConstants
{
    // Matrix Configurations
    public const string UniLevelMatrix             = "UniLevelMatrix";
    public const string ForceMatrix                = "ForceMatrix";
    public const string AffiliatesFrontNum         = "AffiliatesFrontNum";
    public const string SoftwareMillenniumFrontNum = "SoftwareMillenniumFrontNum";
    public const string BinaryMatrix               = "BinaryMatrix";

    // Products Configurations
    public const string ActivatePublicShop            = "ActivatePublicShop";
    public const string ActivateShippingSystem        = "ActivateShippingSystem";
    public const string ActivatePassivePaymentsModule = "ActivatePassivePaymentsModule";
    public const string CurrencySymbol                = "CurrencySymbol";
    public const string SymbolCommissionableValue     = "SymbolCommissionableValue";
    public const string SymbolPointsQualify           = "SymbolPointsQualify";
    public const string BinaryPointsSymbol            = "BinaryPointsSymbol";
    public const string NewProductLabel               = "NewProductLabel";

    // Compensation Plans Configurations
    public const string AutomaticActivation            = "AutomaticActivation";
    public const string AutomaticQualification         = "AutomaticQualification";
    public const string AutomaticIncentiveCalculation  = "AutomaticIncentiveCalculation";
    public const string AutomaticCommissionCalculation = "AutomaticCommissionCalculation";

    // Withdrawals Wallet Configurations
    public const string MinimumAmount               = "MinimumAmount";
    public const string MaximumAmount               = "MaximumAmount";
    public const string CommissionAmount            = "CommissionAmount";
    public const string ActivateInvoiceCancellation = "ActivateInvoiceCancellation";

    // Additional Parameters Wallet Configurations
    public const string MinutesValidityCode       = "MinutesValidityCode";
    public const string ConceptWalletWithdrawal   = "ConceptWalletWithdrawal";
    public const string ActivateConfirmationMails = "ActivateConfirmationMails";

    // General Configurations
    public const string IsUnderMaintenance     = "IsUnderMaintenance";
    public const string PaymentModelCutoffDate = "PaymentModelCutoffDate";
    public const string PointConfiguration     = "Points";
}
