using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record ResendEmailConfirmationCommand(int AffiliateId) : IRequest<bool>;
