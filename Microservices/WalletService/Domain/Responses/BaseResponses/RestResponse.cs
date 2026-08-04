using System.Net;

namespace Ecosystem.WalletService.Domain.Responses.BaseResponses;

public class RestResponse :IRestResponse
{
    public string? Content { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string? StatusDescription { get; set; }
    public bool IsSuccessful => (int)StatusCode is >= 200 and < 300;
}