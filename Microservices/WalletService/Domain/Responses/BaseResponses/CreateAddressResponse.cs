namespace Ecosystem.WalletService.Domain.Responses.BaseResponses;

public class CreateAddressResponse
{
        public Data? Data { get; set; }
        public int StatusCode { get; set; }
        public int IdTypeStatusCode { get; set; }
        public string? Message { get; set; }
}

public class Data
{
        public string? Address { get; set; }
}