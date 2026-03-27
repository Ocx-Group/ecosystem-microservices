using Newtonsoft.Json;


namespace Ecosystem.WalletService.Domain.Responses;

public class UserInfoResponse
{
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("status")] public byte? Status { get; set; }
    
    [JsonProperty("activation_date")]public DateTime? ActivationDate { get; set; }
    [JsonProperty("user_name")] public string? UserName { get; set; }
    [JsonProperty("last_name")] public string? LastName { get; set; }
    [JsonProperty("city")] public string? City { get; set; }
    [JsonProperty("phone")] public string? Phone { get; set; }
    [JsonProperty("email")] public string? Email { get; set; }
    [JsonProperty("address")] public string? Address { get; set; }
    [JsonProperty("affiliate_type")] public string? AffiliateType { get; set; }

    [JsonProperty("card_id_authorization")] public bool? CardIdAuthorization { get; set; }
    [JsonProperty("country_navigation")] public CountryNavigation? Country { get; set; }
    [JsonProperty("father")] public int Father { get; set; }
    [JsonProperty("verification_code")] public string? VerificationCode { get; set; }
}

public class CountryNavigation
{
    [JsonProperty("name")] public string? Name { get; set; }
}