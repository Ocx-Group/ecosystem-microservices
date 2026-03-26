using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.PaymentGroup;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.PaymentGroup;

public class CreatePaymentGroupHandler : IRequestHandler<CreatePaymentGroupCommand, PaymentGroupsDto>
{
    private readonly IPaymentGroupRepository _paymentGroupRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreatePaymentGroupHandler> _logger;

    public CreatePaymentGroupHandler(
        IPaymentGroupRepository paymentGroupRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreatePaymentGroupHandler> logger)
    {
        _paymentGroupRepository = paymentGroupRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaymentGroupsDto> Handle(CreatePaymentGroupCommand request, CancellationToken cancellationToken)
    {
        var paymentGroup = _mapper.Map<PaymentGroups>(request);
        paymentGroup.BrandId = _tenantContext.TenantId;

        var created = await _paymentGroupRepository.CreatePaymentGroups(paymentGroup);
        return _mapper.Map<PaymentGroupsDto>(created);
    }
}
