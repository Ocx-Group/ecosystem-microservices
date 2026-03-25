using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Role;
using Ecosystem.AccountService.Application.DTOs.Role;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Role;

public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, RoleDto?>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public UpdateRoleHandler(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<RoleDto?> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleByIdAsync(request.Id);

        if (role is null)
            return null;

        role.Name = request.Name;
        role.Description = request.Description;

        role = await _roleRepository.UpdateRoleAsync(role);
        return _mapper.Map<RoleDto>(role);
    }
}
