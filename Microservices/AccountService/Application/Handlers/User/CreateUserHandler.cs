using AutoMapper;
using Ecosystem.AccountService.Application.Commands.User;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Helpers;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.User;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public CreateUserHandler(IUserRepository userRepository, ITenantContext tenantContext, IMapper mapper)
    {
        _userRepository = userRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = PasswordHelper.HashPassword(request.Password);

        if (string.IsNullOrEmpty(passwordHash))
            return null;

        var user = new Domain.Models.User
        {
            RolId = request.RolId,
            Username = request.UserName,
            Email = request.Email,
            Password = passwordHash,
            Name = request.Name,
            LastName = request.LastName,
            Phone = request.Phone,
            Address = request.Address,
            Observation = request.Observation,
            Status = request.Status,
            BrandId = _tenantContext.TenantId
        };

        user = await _userRepository.CreateUserAsync(user);
        return _mapper.Map<UserDto>(user);
    }
}
