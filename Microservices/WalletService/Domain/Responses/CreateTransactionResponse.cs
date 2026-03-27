namespace Ecosystem.WalletService.Domain.Responses;

public class CreateTransactionResponse
{
    public TransactionData? Data { get; set; }
    public int StatusCode { get; set; }
    public int IdTypeStatusCode { get; set; }
    public string? CodeMessage { get; set; }
    public string? CodeMessage2 { get; set; }
    public string? Message { get; set; }
    public int IdTransaction { get; set; }  
    public int IdTypeNotificationSent { get; set; }
}

public class TransactionData
{
    public int IdTransaction { get; set; }
    public string? Date { get; set; }
    public int IdUser { get; set; }
    public int IdTeller { get; set; }
    public CurrencyInfo? Currency { get; set; }
    public decimal Amount { get; set; }
    public string? Details { get; set; }
    public string? QrCode { get; set; }
    public object? CustomerNotified { get; set; } 
}

public class CurrencyInfo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public bool IsErc { get; set; }
    public bool IsDigitalCurrency { get; set; }
    public string? Description { get; set; }
}