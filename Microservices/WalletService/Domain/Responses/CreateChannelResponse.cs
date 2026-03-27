
namespace Ecosystem.WalletService.Domain.Responses;
    
    public class CreateChannelResponse
    {
        public ChannelData? Data { get; set; }
        public int StatusCode { get; set; }
        public int IdTypeStatusCode { get; set; }
        public string? Message { get; set; }
    }

    public class ChannelData
    {
        public int Id { get; set; }
        public int IdExternalIdentification { get; set; }
        public string? TagName { get; set; }
        public Currency? Currency { get; set; }
        public string? Address { get; set; }
    }

    public class Currency
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
