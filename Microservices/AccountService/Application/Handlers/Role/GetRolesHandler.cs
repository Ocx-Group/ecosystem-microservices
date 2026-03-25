using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Role;
using Ecosystem.AccountService.Application.Queries.Role;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Role;

public class GetRolesHandler : IRequestHandler<GetRolesQuery, List<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public GetRolesHandler(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<List<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetRolesAsync();
        return _mapper.Map<List<RoleDto>>(roles);
    }
}
