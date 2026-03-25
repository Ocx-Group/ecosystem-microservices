using Ecosystem.AccountService.Application.Commands.Role;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Role;

public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, bool>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleHandler(IRoleRepository roleRepository)
        => _roleRepository = roleRepository;

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleByIdAsync(request.Id);

        if (role is null)
            return false;

        return await _roleRepository.DeleteRoleAsync(role);
    }
}
