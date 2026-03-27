namespace Ecosystem.WalletService.Domain.Responses;

public class GetNetworkResponse
{
    public List<NetworkData>? Data { get; set; }
    public int StatusCode { get; set; }
    public int IdTypeStatusCode { get; set; }
    public string? Message { get; set; }
}

public class NetworkData
{
    public int Id { get; set; }
    public int IdChain { get; set; }
    public string? Name { get; set; }
    public string? ShortName { get; set; }
    public decimal MinimunTransferAmount { get; set; }
    public decimal Fee { get; set; }
}