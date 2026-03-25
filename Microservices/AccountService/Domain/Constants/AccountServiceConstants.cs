namespace Ecosystem.AccountService.Domain.Constants;

public static class AccountServiceConstants
{
    public const string UrlEmailConfirm = "/user_confirm";
    public const string UrlForgotPassword = "/reset";
    public const string SubjectEmailConfirm = "Email Confirmation Notification";
    public const string SubjectVerificationCode = "Verification Code Notification";
    public const string NewContactSubject = "New Contact Notification";
    public const string Admin = "administrador";
    public const string GetPersonalNetworkSP = "account_service.get_personal_network";
    public const string GetBinarySponsor = "account_service.get_binary_sponsor";
    public const string GetBinaryFamilyTree = "account_service.get_binary_family_tree";
    public const string GetModel4FamilyTree = "account_service.get_model4_family_tree";
    public const string GetModel5FamilyTree = "account_service.get_model5_family_tree";
    public const string GetModel6FamilyTree = "account_service.get_model6_family_tree";
    public const string GetUniLevelFamilyTree = "account_service.get_unilevel_family_tree";
    public const int EcoPoolLevels = 5;
    public const int LevelsTree = 3;
    public const string SubjectPasswordRecovery = "Recuperación de contraseña";
    public const string SubjectPasswordChangeConfirmation = "Restablecimiento de contraseña exitoso";
    public const string GetTotalAffiliatesByCountries = "account_service.get_total_affiliates_by_country()";
    public const int EcosystemId = 1;
    public const int RecyCoinId = 2;
    public const int HouseCoinId = 3;
    public const int ExitoJuntosId = 4;
    public const int FatherRecyCoin = 12557;
    public const int FatherExitoJuntos = 12673;
    public const int FatherHouseCoin = 12586;
    public const string EcosystemSenderName = "Ecosystem Sharing Evolution";
    public const string RecyCoinSenderName = "Recycoin";
    public const string HouseCoinSenderName = "Housecoin";
    public const string ExitoJuntosSenderName = "Éxito Juntos";

    public enum AffiliateStatus
    {
        Confirmación_Pendiente,
        Identificación_Pendiente,
        Aprobación_Pendiente,
        Affiliado_Aprobado,
    }

    public static readonly string[] BinarySideArray =
    [
        "Pierna Libre",
        "Pierna Izquierda",
        "Pierna Derecha"
    ];

    public enum CardIdStatus
    {
        Aprobado,
        Rechazado,
        Pendiente,
    }
}