using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.User;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.User;

public class GetUsersByRolIdHandler : IRequestHandler<GetUsersByRolIdQuery, ICollection<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetUsersByRolIdHandler(IUserRepository userRepository, ITenantContext tenantContext, IMapper mapper)
    {
        _userRepository = userRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<ICollection<UserDto>> Handle(GetUsersByRolIdQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetUserByRolIdAsync(request.RolId, _tenantContext.TenantId);
        return _mapper.Map<ICollection<UserDto>>(users);
    }
}
