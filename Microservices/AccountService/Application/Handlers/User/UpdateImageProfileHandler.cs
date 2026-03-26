using AutoMapper;
using Ecosystem.AccountService.Application.Commands.User;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.User;

public class UpdateImageProfileHandler : IRequestHandler<UpdateImageProfileCommand, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public UpdateImageProfileHandler(IUserRepository userRepository, ITenantContext tenantContext, IMapper mapper)
    {
        _userRepository = userRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(UpdateImageProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.Id, _tenantContext.TenantId);

        if (user is null)
            return null;

        user.ImageProfileUrl = request.ImageProfileUrl;
        user = await _userRepository.UpdateUserImage(user);

        return _mapper.Map<UserDto>(user);
    }
}
