using Ecosystem.AccountService.Application.DTOs.Privilege;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Privilege;

public record GetAllPrivilegesQuery() : IRequest<ICollection<PrivilegesDto>>;
