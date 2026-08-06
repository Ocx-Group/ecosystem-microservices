using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.DTOs.PaginationDto;
using Ecosystem.AccountService.Domain.Requests.PaginationRequest;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Affiliate;

public record GetAffiliatesPagedQuery(PaginationRequest Request) : IRequest<PaginationDto<UsersAffiliatesDto>>;
