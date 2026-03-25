namespace Ecosystem.AccountService.Domain.Models;

public partial class AffiliatesAddress
{
    public long Id { get; set; }
    public long AffiliateId { get; set; }
    public string? FiscalIdentification { get; set; }
    public string AddressName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Company { get; set; }
    public string? IvaNumber { get; set; }
    public string Address { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public int Country { get; set; }
    public string LandlinePhone { get; set; } = null!;
    public string MobilePhone { get; set; } = null!;
    public string? Other { get; set; }
    public DateTime Date { get; set; }
    public string CountryName { get; set; } = null!;
    public string? StateName { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual UsersAffiliate Affiliate { get; set; } = null!;
    public virtual Country CountryNavigation { get; set; } = null!;
}
