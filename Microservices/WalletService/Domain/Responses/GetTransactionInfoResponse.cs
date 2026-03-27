namespace Ecosystem.WalletService.Domain.Responses;

public class GetTransactionInfoResponse
{
    public string Error { get; set; }
    public TransactionResult Result { get; set; }
}

public class TransactionResult
{
    public long TimeCreated { get; set; }
    public long TimeExpires { get; set; }
    public int Status { get; set; }
    public string StatusText { get; set; }
    public string Type { get; set; }
    public string Coin { get; set; }
    public long Amount { get; set; }
    public string Amountf { get; set; }
    public long Received { get; set; }
    public string Receivedf { get; set; }
    public int RecvConfirms { get; set; }
    public string PaymentAddress { get; set; }
    public string SenderIp { get; set; }
    public CheckoutInfo Checkout { get; set; }
    public List<object> Shipping { get; set; } 
}

public class CheckoutInfo
{
    public string Currency { get; set; }
    public long Amount { get; set; }
    public int Test { get; set; }
    public string ItemNumber { get; set; }
    public string ItemName { get; set; }
    public List<object> Details { get; set; }
    public string Invoice { get; set; }
    public string Custom { get; set; }
    public string IpnUrl { get; set; }
    public decimal Amountf { get; set; }
}