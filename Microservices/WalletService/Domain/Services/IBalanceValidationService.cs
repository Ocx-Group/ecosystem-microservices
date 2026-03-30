namespace Ecosystem.WalletService.Domain.Services;

public record BalanceValidationResult(
    bool IsSuccess,
    decimal AvailableBalance = 0,
    string? ErrorMessage = null
)
{
    public static BalanceValidationResult Success(decimal balance) => new(true, balance);
    public static BalanceValidationResult InsufficientFunds(decimal available) =>
        new(false, available, $"Saldo insuficiente. Disponible: {available}");
}

public interface IBalanceValidationService
{
    Task<BalanceValidationResult> ValidateBalance(int affiliateId, long brandId, decimal requiredAmount);
}
