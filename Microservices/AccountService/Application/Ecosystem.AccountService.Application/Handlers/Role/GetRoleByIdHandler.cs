using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Role;
using Ecosystem.AccountService.Application.Queries.Role;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Role;

public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public GetRoleByIdHandler(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleByIdAsync(request.Id);
        return role is null ? null : _mapper.Map<RoleDto>(role);
    }
}
