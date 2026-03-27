namespace Ecosystem.WalletService.Domain.Responses;

public class GetDepositAddressResponse
{
    public string? Error { get; set; }
    public AddressResult? Result { get; set; }
}

public class AddressResult
{
    public string Address { get; set; } = string.Empty;
}