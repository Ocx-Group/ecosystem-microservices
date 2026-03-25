using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Ecosystem.AccountService.Domain.Models.CustomModels;

public class AffiliatePersonalNetwork
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [Column("full_name")]
    [JsonProperty("fullName")]
    public string FullName { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [Column("user_name")]
    [JsonProperty("userName")]
    public string UserName { get; set; }

    [Column("country_name")]
    [JsonProperty("countryName")]
    public string CountryName { get; set; }

    [JsonProperty("latitude")]
    public decimal Latitude { get; set; }

    [JsonProperty("longitude")]
    public decimal Longitude { get; set; }

    [Column("external_grading_id")]
    [JsonProperty("externalGradingId")]
    public int ExternalGradingId { get; set; }

    [Column("external_grading_id_before")]
    [JsonProperty("externalGradingIdBefore")]
    public int ExternalGradingIdBefore { get; set; }

    [JsonProperty("father")]
    public int Father { get; set; }

    [JsonProperty("status")]
    public short Status { get; set; }

    [Column("activation_date")]
    [JsonProperty("activationDate")]
    public DateTime? ActivationDate { get; set; }
}
