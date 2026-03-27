using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.Responses;

public class GetPayByNameProfileResponse
{

    public string Error { get; set; } = string.Empty;
    public ProfileResult? Result { get; set; }
}

public class ProfileResult
{
    [JsonPropertyName("pbntag")]
    public string? Pbntag { get; set; }

    [JsonPropertyName("merchant")]
    public string? Merchant { get; set; } 

    [JsonPropertyName("profile_name")]
    public string? ProfileName { get; set; } 

    [JsonPropertyName("profile_url")]
    public string? ProfileUrl { get; set; } 

    [JsonPropertyName("profile_email")]
    public string? ProfileEmail { get; set; }

    [JsonPropertyName("profile_image")]
    public string? ProfileImage { get; set; } 

    [JsonPropertyName("member_since")]
    public long MemberSince { get; set; }

    [JsonPropertyName("feedback")]
    public Feedback? Feedback { get; set; }

}

public class Feedback
{
    [JsonPropertyName("pos")]
    public int Pos { get; set; }

    [JsonPropertyName("neg")]
    public int Neg { get; set; }

    [JsonPropertyName("neut")]
    public int Neut { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("percent")]
    public string Percent { get; set; } = string.Empty;

    [JsonPropertyName("percent_str")]
    public string PercentStr { get; set; } = string.Empty;
}