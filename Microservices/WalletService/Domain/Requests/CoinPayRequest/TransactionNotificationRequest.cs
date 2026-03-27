namespace Ecosystem.WalletService.Domain.Requests.CoinPayRequest;

public class TransactionNotificationRequest
{
    public int IdUser { get; set; }
    public int IdWallet { get; set; }
    public int IdTransaction { get; set; }
    public string? IdExternalReference { get; set; }
    public string? Address { get; set; }
    public string? TxtId { get; set; }
    public decimal Amount { get; set; }
    public int Fee { get; set; }
    public int ExchangeRate { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public Currency? Currency { get; set; }
    public UserChannel? UserChannel { get; set; }
    public TransactionType? TransactionType { get; set; }
    public TransactionStatus? TransactionStatus { get; set; }
    public int NumberAttemps { get; set; }
    public int WalletConfirmations { get; set; }
    public string? CreateDate { get; set; }
}

public class PaymentMethod
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class Currency
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
}

public class UserChannel
{
    public string? IdExternalIdentification { get; set; }
    public string? TagName { get; set; }
}

public class TransactionType
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class TransactionStatus
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}