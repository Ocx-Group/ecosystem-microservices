using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Replace direct Brevo email calls with event bus / gRPC
public interface IEmailServiceAdapter
{
    Task SendInvitationsForTradingAcademy(UserAffiliateResponse user, string link, string code, long brandId);
}
