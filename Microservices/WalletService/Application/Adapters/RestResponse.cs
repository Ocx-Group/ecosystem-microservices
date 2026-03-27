namespace Ecosystem.WalletService.Application.Adapters;

public class RestResponse
{
    public bool IsSuccessful { get; set; }
    public string? Content { get; set; }
    public int StatusCode { get; set; }
}
