using Ecosystem.AccountService.Application.Commands.Auth;
using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.DTOs.Auth;
using Ecosystem.AccountService.Application.Helpers;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Auth;

public class UserAuthenticationHandler : IRequestHandler<UserAuthenticationCommand, AuthResultDto>
{
    private readonly IUserAffiliateInfoRepository _userAffiliateInfoRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILoginMovementsRepository _loginMovementsRepository;
    private readonly IMasterPasswordRepository _masterPasswordRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UserAuthenticationHandler> _logger;

    public UserAuthenticationHandler(
        IUserAffiliateInfoRepository userAffiliateInfoRepository,
        IUserRepository userRepository,
        ILoginMovementsRepository loginMovementsRepository,
        IMasterPasswordRepository masterPasswordRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UserAuthenticationHandler> logger)
    {
        _userAffiliateInfoRepository = userAffiliateInfoRepository;
        _userRepository = userRepository;
        _loginMovementsRepository = loginMovementsRepository;
        _masterPasswordRepository = masterPasswordRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AuthResultDto> Handle(UserAuthenticationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[UserAuthenticationHandler] | Request: {UserName}", request.UserName);

        var masterOk = await ValidateWithMasterPassword(request.Password);

        // 1) Try affiliate login
        var affiliate = await _userAffiliateInfoRepository.GetAffiliateByUserNameAuthAsync(
            request.UserName, _tenantContext.TenantId);

        if (affiliate is not null && (masterOk || PasswordHelper.VerifyPassword(affiliate.Password, request.Password)))
        {
            _logger.LogInformation("[UserAuthenticationHandler] | Affiliate validated: {UserName}", request.UserName);

            var movement = _mapper.Map<LoginMovement>(request);
            movement.AffiliateId = affiliate.Id;
            movement.BrandId = _tenantContext.TenantId;
            await _loginMovementsRepository.CreateAsync(movement);

            return new AuthResultDto { Affiliate = _mapper.Map<UsersAffiliatesDto>(affiliate) };
        }

        if (affiliate is not null)
            return new AuthResultDto();

        // 2) Try user login
        var user = await _userRepository.GetUserByUserNameAsync(
            request.UserName, _tenantContext.TenantId);

        if (user is not null && (masterOk || PasswordHelper.VerifyPassword(user.Password, request.Password)))
        {
            _logger.LogInformation("[UserAuthenticationHandler] | User validated: {UserName}", request.UserName);

            var movement = _mapper.Map<LoginMovement>(request);
            movement.AffiliateId = user.Id;
            movement.BrandId = _tenantContext.TenantId;
            await _loginMovementsRepository.CreateAsync(movement);

            return new AuthResultDto { User = _mapper.Map<UserDto>(user) };
        }

        return new AuthResultDto();
    }

    private async Task<bool> ValidateWithMasterPassword(string inputPassword)
    {
        var tenantId = _tenantContext.TenantId;
        var masterPassword = await _masterPasswordRepository.GetMasterPasswordByBrandId((int)tenantId);

        return masterPassword is not null && PasswordHelper.VerifyPassword(masterPassword.Password, inputPassword);
    }
}
