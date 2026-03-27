namespace Ecosystem.WalletService.Domain.Responses.BaseResponses;

public class CoinPayResponse
{
    public DataModel Data { get; set; }
    public int StatusCode { get; set; }
    public int IdTypeStatusCode { get; set; }
    public string Message { get; set; }
    public string[] Messages { get; set; }
}

public class DataModel
{
    public int IdUser { get; set; }
    public string Token { get; set; }
}