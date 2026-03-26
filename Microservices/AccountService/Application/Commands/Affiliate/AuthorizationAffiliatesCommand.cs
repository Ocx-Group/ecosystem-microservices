using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record AuthorizationAffiliatesCommand(long[] ApprovedArray, long[] DisApprovedArray) : IRequest<bool>;
