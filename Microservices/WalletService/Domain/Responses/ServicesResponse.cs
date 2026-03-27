using System.Text.Json.Serialization;
using Ecosystem.WalletService.Domain.DTOs.AffiliateBtc;
using Ecosystem.WalletService.Domain.DTOs.AffiliateInformation;
using Ecosystem.WalletService.Domain.DTOs.ProductWalletDto;
using NewtonsoftJson = Newtonsoft.Json;

namespace Ecosystem.WalletService.Domain.Responses;

public class ServicesResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("data")] public object? Data { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;

    [JsonPropertyName("code")] public int Code { get; set; }
}

public class ServicesValidCodeAccountResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("data")] public object? Data { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;

    [JsonPropertyName("code")] public int Code { get; set; }
}

public class ProductsResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("data")] public List<ProductWalletDto> Data { get; set; }
    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
    [JsonPropertyName("code")] public int Code { get; set; }
}

public class ProductResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("data")] public ProductWalletDto Data { get; set; }
    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
    [JsonPropertyName("code")] public int Code { get; set; }
}

public class UserAffiliateResponse
{ 
    [NewtonsoftJson.JsonProperty] public bool Success { get; set; }

    [NewtonsoftJson.JsonProperty("data")] public UserInfoResponse? Data { get; set; }

    [NewtonsoftJson.JsonProperty("message")] public string Message { get; set; } = string.Empty;

    [NewtonsoftJson.JsonProperty("code")] public int Code { get; set; }
    
}

public class UserAffiliatePointInformation
{ 
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("data")] public ICollection<UserBinaryInformation> Data { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;

    [JsonPropertyName("code")] public int Code { get; set; }
    
}

public class UserPersonalNetworkResponse
{ 
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("data")] public List<PersonalNetwork> Data { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;

    [JsonPropertyName("code")] public int Code { get; set; }
    
}

public class GetTotalActiveMembersResponse
{ 
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("data")] public int Data { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;

    [JsonPropertyName("code")] public int Code { get; set; }
    
}

public class AffiliateBtcResponse
{
    [NewtonsoftJson.JsonProperty("success")] public bool Success { get; set; }

    [NewtonsoftJson.JsonProperty("data")] public AffiliateBtcDto?[]? Data { get; set; }

    [NewtonsoftJson.JsonProperty("message")] public string Message { get; set; } = string.Empty;

    [NewtonsoftJson.JsonProperty("code")] public int Code { get; set; }
}

public class PersonalNetwork
{

    public int id { get; set; }
    public string fullName { get; set; }
    public string email { get; set; }
    public string userName { get; set; }
    public int externalGradingId { get; set; }
    public byte status { get; set; }
    public decimal latitude { get; set; }
    public decimal longitude { get; set; }
    public string countryName { get; set; }
}
