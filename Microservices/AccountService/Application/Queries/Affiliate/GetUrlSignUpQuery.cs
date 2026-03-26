using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Affiliate;

public record GetUrlSignUpQuery(int UserId) : IRequest<string?>;
