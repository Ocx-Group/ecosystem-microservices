using AutoMapper;
using Ecosystem.AccountService.Application.Commands.User;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.User;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public UpdateUserHandler(IUserRepository userRepository, ITenantContext tenantContext, IMapper mapper)
    {
        _userRepository = userRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.Id, _tenantContext.TenantId);

        if (user is null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Email))
            return null;

        user.Username = request.UserName ?? "";
        user.Name = request.Name;
        user.LastName = request.LastName;
        user.Email = request.Email ?? "";
        user.RolId = request.RolId;
        user.Phone = request.Phone;
        user.Address = request.Address;
        user.Observation = request.Observation;
        user.Status = request.Status;
        user.BrandId = _tenantContext.TenantId;

        user = await _userRepository.UpdateUserAsync(user);
        return _mapper.Map<UserDto>(user);
    }
}
