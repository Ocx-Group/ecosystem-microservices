namespace Ecosystem.WalletService.Domain.Responses;

public partial class GetTransactionByIdResponse
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public int IdWallet { get; set; }
    public int Amount { get; set; }
    public int Fee { get; set; }
    public int ExchangeRate { get; set; }
    public int ExchangeRateFlat { get; set; }
    public string Address { get; set; }
    public string TxtId { get; set; }
    public string Description { get; set; }
    public Currency Currency { get; set; }
    public TransactionType TransactionType { get; set; }
    public TransactionStatus TransactionStatus { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime CreateTransaction { get; set; }
    public DateTime ModifiedTransaction { get; set; }
}
public class TransactionType
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class TransactionStatus
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class PaymentMethod
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public partial class GetTransactionByIdResponse
{
    public TransactionData Data { get; set; }
    public int StatusCode { get; set; }
    public int IdTypeStatusCode { get; set; }
    public string Message { get; set; }
}