using AutoMapper;
using Ecosystem.AccountService.Application.Commands.User;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Helpers;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.User;

public class UpdatePasswordHandler : IRequestHandler<UpdatePasswordCommand, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public UpdatePasswordHandler(IUserRepository userRepository, ITenantContext tenantContext, IMapper mapper)
    {
        _userRepository = userRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.Id, _tenantContext.TenantId);

        if (user is null)
            return null;

        if (!PasswordHelper.VerifyPassword(user.Password, request.Password))
            return null;

        user.Password = PasswordHelper.HashPassword(request.NewPassword);
        user = await _userRepository.UpdateUserAsync(user);

        return _mapper.Map<UserDto>(user);
    }
}
