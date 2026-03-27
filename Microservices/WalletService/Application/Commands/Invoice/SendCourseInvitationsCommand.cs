using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Invoice;

public record SendCourseInvitationsCommand(string Link, string Code) : IRequest<IEnumerable<UserAffiliateResponse>>;
