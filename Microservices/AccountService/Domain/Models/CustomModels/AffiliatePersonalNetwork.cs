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

    /// <summary>
    /// Telefono del afiliado. La funcion account_service.get_personal_network no lo
    /// devuelve: por eso va [NotMapped] (si no, EF lo buscaria como columna del
    /// resultado y la consulta fallaria) y se completa en GetPersonalNetworkHandler.
    /// </summary>
    [NotMapped]
    [JsonProperty("phone")]
    public string? Phone { get; set; }

    /// <summary>
    /// Profundidad del afiliado dentro de la red consultada. Los afiliados
    /// directos pertenecen al nivel 1.
    /// </summary>
    [NotMapped]
    [JsonProperty("level")]
    public int Level { get; set; }
}
