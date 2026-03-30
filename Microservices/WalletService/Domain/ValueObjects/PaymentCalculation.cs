namespace Ecosystem.WalletService.Domain.ValueObjects;

public record PaymentCalculation(
    decimal Debit,
    decimal Points,
    decimal Commissionable,
    short Origin,
    decimal TotalBaseAmount
);
