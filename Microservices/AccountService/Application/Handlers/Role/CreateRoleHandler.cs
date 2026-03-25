using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Role;
using Ecosystem.AccountService.Application.DTOs.Role;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Role;

public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, RoleDto?>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public CreateRoleHandler(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<RoleDto?> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = _mapper.Map<Domain.Models.Role>(request);
        role = await _roleRepository.CreateRoleAsync(role);
        return _mapper.Map<RoleDto>(role);
    }
}
