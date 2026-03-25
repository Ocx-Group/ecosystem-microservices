using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Auth;

public record GetCountriesQuery : IRequest<ICollection<CountryDto>>;
