using AutoMapper;
using Ecosystem.AccountService.Application.Adapters;
using Ecosystem.AccountService.Application.Commands.Auth;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.DTOs.Auth;
using Ecosystem.AccountService.Application.Helpers;
using Ecosystem.AccountService.Application.Settings;
using Ecosystem.AccountService.Domain.Constants;
using Ecosystem.AccountService.Domain.Enums;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.Domain.Core.Bus;
using Ecosystem.Domain.Core.Events;
using Ecosystem.Domain.Core.MultiTenancy;
using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ecosystem.AccountService.Application.Handlers.Auth;

public class GoogleAuthenticationHandler : IRequestHandler<GoogleAuthenticationCommand, AuthResultDto>
{
    private readonly IUserAffiliateInfoRepository _affiliateRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILoginMovementsRepository _loginMovementsRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;
    private readonly GoogleAuthSettings _settings;
    private readonly ILogger<GoogleAuthenticationHandler> _logger;
    private readonly IConfigurationServiceAdapter _configurationServiceAdapter;

    public GoogleAuthenticationHandler(
        IUserAffiliateInfoRepository affiliateRepository,
        IUserRepository userRepository,
        ILoginMovementsRepository loginMovementsRepository,
        ITenantContext tenantContext,
        IEventBus eventBus,
        IMapper mapper,
        IOptions<GoogleAuthSettings> settings,
        IConfigurationServiceAdapter configurationServiceAdapter,
        ILogger<GoogleAuthenticationHandler> logger)
    {
        _affiliateRepository = affiliateRepository;
        _userRepository = userRepository;
        _loginMovementsRepository = loginMovementsRepository;
        _tenantContext = tenantContext;
        _eventBus = eventBus;
        _mapper = mapper;
        _settings = settings.Value;
        _configurationServiceAdapter = configurationServiceAdapter;
        _logger = logger;
    }

    public async Task<AuthResultDto> Handle(GoogleAuthenticationCommand request, CancellationToken cancellationToken)
    {
        var payload = await ValidateGoogleToken(request.IdToken);
        if (payload is null)
            return Fail("No fue posible validar la cuenta de Google.");

        if (payload.EmailVerified != true)
            return Fail("La cuenta de Google no tiene el correo verificado.");

        var brandId = _tenantContext.TenantId;
        var googleSubject = payload.Subject;
        var email = payload.Email.Trim().ToLowerInvariant();

        var affiliate = await _affiliateRepository.GetAffiliateByGoogleAuthCodeAsync(googleSubject, brandId);
        if (affiliate is not null)
            return await CompleteAffiliateLogin(request, affiliate, brandId);

        affiliate = await _affiliateRepository.GetAffiliateByEmailAsync(email, brandId);
        if (affiliate is not null)
        {
            if (affiliate.Status != 1)
                return Fail("El usuario existe, pero no está activo.");

            affiliate.GoogleAuthCode = googleSubject;
            affiliate.IsGoogleAuth = true;
            await _affiliateRepository.UpdateAffiliateAsync(affiliate);
            return await CompleteAffiliateLogin(request, affiliate, brandId);
        }

        var user = await _userRepository.GetUserByEmailAsync(email, brandId);
        if (user is not null)
        {
            if (user.Status is false)
                return Fail("El usuario existe, pero no está activo.");

            return await CompleteAdminLogin(request, user, brandId);
        }

        if (string.IsNullOrWhiteSpace(request.ReferralUserName))
            return Fail("Esta cuenta de Google no está registrada. Use un enlace de referido para registrarse.");

        var sponsor = await _affiliateRepository.GetAffiliateByUserNameAsync(request.ReferralUserName, brandId);
        if (sponsor is null)
            return Fail("El referido no existe.");

        if (request.Country is null or <= 0)
            return Fail("El país es requerido para registrarse con Google.");

        if (!request.TermsConditions)
            return Fail("Debe aceptar los términos y condiciones.");

        var brandConfiguration = await _configurationServiceAdapter
            .GetBrandConfigurationAsync(brandId, cancellationToken);
        if (brandConfiguration is null)
            return Fail("La configuración de la marca no está disponible.");

        var userName = await GenerateAvailableUserName(email, brandId);
        var newAffiliate = await CreateAffiliateFromGoogle(
            request,
            payload,
            userName,
            sponsor,
            brandId,
            brandConfiguration.ActivateOnRegistration);

        return await CompleteAffiliateLogin(request, newAffiliate, brandId);
    }

    private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string idToken)
    {
        var clientIds = _settings.ClientIds
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (clientIds.Length == 0)
        {
            _logger.LogWarning("GoogleAuth:ClientIds is not configured.");
            return null;
        }

        try
        {
            return await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = clientIds
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Google token validation failed.");
            return null;
        }
    }

    private async Task<AuthResultDto> CompleteAffiliateLogin(
        GoogleAuthenticationCommand request,
        UsersAffiliate affiliate,
        long brandId)
    {
        var movement = _mapper.Map<LoginMovement>(request);
        movement.AffiliateId = affiliate.Id;
        movement.BrandId = brandId;
        await _loginMovementsRepository.CreateAsync(movement);

        return new AuthResultDto { Affiliate = _mapper.Map<UsersAffiliatesDto>(affiliate) };
    }

    private async Task<AuthResultDto> CompleteAdminLogin(
        GoogleAuthenticationCommand request,
        Domain.Models.User user,
        long brandId)
    {
        var movement = _mapper.Map<LoginMovement>(request);
        movement.AffiliateId = user.Id;
        movement.BrandId = brandId;
        await _loginMovementsRepository.CreateAsync(movement);

        return new AuthResultDto { User = _mapper.Map<UserDto>(user) };
    }

    private async Task<UsersAffiliate> CreateAffiliateFromGoogle(
        GoogleAuthenticationCommand request,
        GoogleJsonWebSignature.Payload payload,
        string userName,
        UsersAffiliate sponsor,
        long brandId,
        bool activateOnRegistration)
    {
        var affiliateType = request.ReferralUserName is null ? null : sponsor.AffiliateType;

        var affiliate = new UsersAffiliate
        {
            Username = userName,
            Name = payload.GivenName ?? payload.Name,
            LastName = payload.FamilyName,
            Email = payload.Email.Trim().ToLowerInvariant(),
            Password = PasswordHelper.HashPassword(Guid.NewGuid().ToString("N")),
            Country = request.Country!.Value,
            AffiliateType = affiliateType,
            Father = (int)sponsor.Id,
            Sponsor = (int)sponsor.Id,
            BinarySponsor = (int)sponsor.Id,
            Phone = request.Phone,
            StatePlace = string.Empty,
            City = string.Empty,
            BinaryMatrixSide = 1,
            Status = 1,
            EmailVerification = true,
            AffiliateMode = 1,
            CardIdAuthorization = false,
            Identification = string.Empty,
            PrivateKey = string.Empty,
            ExternalProductId = 0,
            ExternalGradingId = 1,
            ExternalGradingIdBefore = 0,
            Side = sponsor.BinaryMatrixSide,
            StatusActivation = nameof(AccountServiceConstants.AffiliateStatus.Confirmación_Pendiente),
            TermsConditions = true,
            BrandId = brandId,
            ActivationDate = activateOnRegistration ? DateTime.Now : null,
            GoogleAuthCode = payload.Subject,
            IsGoogleAuth = true,
            ImageProfileUrl = payload.Picture
        };

        affiliate = await _affiliateRepository.CreateAffiliateAsync(affiliate);
        affiliate.VerificationCode = VerificationCodeHelper.GenerateVerificationCode(affiliate.Id);
        await _affiliateRepository.UpdateVerificationCodeAffiliateAsync(affiliate);

        await _eventBus.Publish(new SendEmailEvent(
            "email_validation",
            brandId,
            affiliate.Email,
            affiliate.Username,
            new Dictionary<string, string>
            {
                { "userName", affiliate.Username },
                { "verificationCode", affiliate.VerificationCode ?? string.Empty }
            }));

        return affiliate;
    }

    private async Task<string> GenerateAvailableUserName(string email, long brandId)
    {
        var baseName = NormalizeUserName(email.Split('@')[0]);

        for (var i = 0; i < 100; i++)
        {
            var candidate = i == 0 ? baseName : $"{baseName}{i}";
            var status = await _affiliateRepository.CheckAffiliateExistenceAsync(email, candidate, brandId);
            if (status is ExistenceStatus.None or ExistenceStatus.EmailExists)
                return candidate;
        }

        return $"{baseName}{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
    }

    private static string NormalizeUserName(string value)
    {
        var chars = value
            .ToLowerInvariant()
            .Where(char.IsLetterOrDigit)
            .ToArray();

        var normalized = new string(chars);
        return normalized.Length >= 4 ? normalized : $"user{normalized}";
    }

    private static AuthResultDto Fail(string message)
        => new() { ErrorMessage = message };
}
