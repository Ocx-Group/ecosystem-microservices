namespace Ecosystem.AccountService.Domain.Models.CustomModels;

public class CountryNetwork
{
    public string Title { get; set; } = string.Empty;
    public long Value { get; set; }
    public decimal Lat { get; set; }
    public decimal Lng { get; set; }
}
