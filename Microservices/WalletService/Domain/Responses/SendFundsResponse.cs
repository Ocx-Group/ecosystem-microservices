namespace Ecosystem.WalletService.Domain.Responses;

public class SendFundsResponse
{
    public DataDetails? Data { get; set; }
    public int StatusCode { get; set; }
    public int IdTypeStatusCode { get; set; }
    public string? Message { get; set; }

    public class DataDetails
    {
        public int IdTransaction { get; set; }
        public int IdWallet { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public CurrencyDetails? Currency { get; set; }
        public string? Address { get; set; }
    }

    public class CurrencyDetails
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public bool IsErc { get; set; }
    }
}