using AutoMapper;
using Ecosystem.AccountService.Application.Adapters;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Helpers;
using Ecosystem.AccountService.Domain.Constants;
using Ecosystem.AccountService.Domain.Enums;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.Domain.Core.Bus;
using Ecosystem.Domain.Core.Events;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class CreateAffiliateHandler : IRequestHandler<CreateAffiliateCommand, ServicesResponse>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;
    private readonly IConfigurationServiceAdapter _configurationServiceAdapter;

    public CreateAffiliateHandler(
        IUserAffiliateInfoRepository repo,
        ITenantContext tenantContext,
        IEventBus eventBus,
        IMapper mapper,
        IConfigurationServiceAdapter configurationServiceAdapter)
    {
        _repo = repo;
        _tenantContext = tenantContext;
        _eventBus = eventBus;
        _mapper = mapper;
        _configurationServiceAdapter = configurationServiceAdapter;
    }

    public async Task<ServicesResponse> Handle(CreateAffiliateCommand request, CancellationToken ct)
    {
        var brandId = _tenantContext.TenantId;
        var brandConfiguration = await _configurationServiceAdapter
            .GetBrandConfigurationAsync(brandId, ct);

        if (brandConfiguration is null)
        {
            return new ServicesResponse
            {
                Success = false,
                Message = $"Active brand configuration not found for brand {brandId}.",
                Code = 503
            };
        }

        var father = request.Father;
        var sponsor = request.Sponsor;
        var binarySponsor = request.BinarySponsor;
        var affiliateType = request.AffiliateType;

        if (father == 0 &&
            brandConfiguration.DefaultFatherAffiliateId is int defaultFatherAffiliateId)
        {
            father = defaultFatherAffiliateId;
            sponsor = defaultFatherAffiliateId;
            binarySponsor = defaultFatherAffiliateId;
            affiliateType = "01_Membresia_10";
        }

        var existenceStatus = await _repo.CheckAffiliateExistenceAsync(request.Email, request.UserName, brandId);

        switch (existenceStatus)
        {
            case ExistenceStatus.EmailExists:
                return new ServicesResponse { Success = false, Message = "El correo electrónico se encuentra registrado.", Code = 400 };
            case ExistenceStatus.UserNameExists:
                return new ServicesResponse { Success = false, Message = "El nombre de usuario se encuentra registrado.", Code = 400 };
            case ExistenceStatus.BothExist:
                return new ServicesResponse { Success = false, Message = "Correo electrónico y nombre de usuario se encuentran registrados.", Code = 400 };
        }

        var passwordHash = PasswordHelper.HashPassword(request.Password);
        if (string.IsNullOrEmpty(passwordHash))
            return new ServicesResponse { Success = false, Message = "Error encrypting password.", Code = 500 };

        var fatherEntity = await _repo.GetAffiliateByIdAsync(father, brandId);
        if (fatherEntity is null)
            return new ServicesResponse { Success = false, Message = "Father affiliate not found.", Code = 400 };

        var affiliate = new UsersAffiliate
        {
            Username = request.UserName,
            Name = request.Name,
            LastName = request.LastName,
            Email = request.Email.ToLower(),
            Password = passwordHash,
            Country = request.Country,
            AffiliateType = affiliateType,
            Father = father,
            Sponsor = sponsor ?? father,
            BinarySponsor = binarySponsor ?? father,
            Phone = request.Phone,
            StatePlace = request.StatePlace,
            City = request.City,
            BinaryMatrixSide = 1,
            Status = request.Status ?? 0,
            EmailVerification = true,
            AffiliateMode = 1,
            CardIdAuthorization = false,
            Identification = string.Empty,
            PrivateKey = string.Empty,
            ExternalProductId = 0,
            ExternalGradingId = 1,
            ExternalGradingIdBefore = 0,
            Side = fatherEntity.BinaryMatrixSide,
            StatusActivation = nameof(AccountServiceConstants.AffiliateStatus.Confirmación_Pendiente),
            TermsConditions = true,
            BrandId = brandId,
            ActivationDate = brandConfiguration.ActivateOnRegistration
                ? DateTime.Now
                : null
        };

        affiliate = await _repo.CreateAffiliateAsync(affiliate);
        affiliate.VerificationCode = VerificationCodeHelper.GenerateVerificationCode(affiliate.Id);
        await _repo.UpdateVerificationCodeAffiliateAsync(affiliate);

        // Send email validation via NotificationService
        await _eventBus.Publish(new SendEmailEvent(
            "email_validation",
            brandId,
            affiliate.Email,
            affiliate.Username,
            new Dictionary<string, string>
            {
                { "userName", affiliate.Username },
                { "verificationCode", affiliate.VerificationCode ?? "" }
            }));

        var dto = _mapper.Map<UsersAffiliatesDto>(affiliate);
        return new ServicesResponse { Success = true, Data = dto, Message = "El usuario se registró correctamente.", Code = 200 };
    }
}
