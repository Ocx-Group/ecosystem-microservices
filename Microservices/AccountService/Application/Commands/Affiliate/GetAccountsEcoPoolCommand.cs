using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record GetAccountsEcoPoolCommand(long[] AffiliatesIds, int Levels) : IRequest<ICollection<UserEcoPoolDto>>;
