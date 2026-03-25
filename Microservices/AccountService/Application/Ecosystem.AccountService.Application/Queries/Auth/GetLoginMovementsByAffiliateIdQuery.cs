using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Auth;

public record GetLoginMovementsByAffiliateIdQuery(int AffiliateId) : IRequest<List<LoginMovementsDto>>;
