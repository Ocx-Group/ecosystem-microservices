namespace Ecosystem.AccountService.Application.DTOs.AffiliateAddress;

public class AffiliateAddressDto
{
    public int Id { get; set; }
    public int AffiliateId { get; set; }
    public string? FiscalIdentification { get; set; }
    public string AddressName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? IvaNumber { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public int Country { get; set; }
    public string LandlinePhone { get; set; } = string.Empty;
    public string MobilePhone { get; set; } = string.Empty;
    public string? Other { get; set; }
    public DateTime Date { get; set; }
    public string CountryName { get; set; } = string.Empty;
    public string? StateName { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
