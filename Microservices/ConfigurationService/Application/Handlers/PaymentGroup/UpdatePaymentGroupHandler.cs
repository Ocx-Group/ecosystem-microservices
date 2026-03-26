using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.PaymentGroup;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.PaymentGroup;

public class UpdatePaymentGroupHandler : IRequestHandler<UpdatePaymentGroupCommand, PaymentGroupsDto?>
{
    private readonly IPaymentGroupRepository _paymentGroupRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdatePaymentGroupHandler> _logger;

    public UpdatePaymentGroupHandler(
        IPaymentGroupRepository paymentGroupRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UpdatePaymentGroupHandler> logger)
    {
        _paymentGroupRepository = paymentGroupRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaymentGroupsDto?> Handle(UpdatePaymentGroupCommand request, CancellationToken cancellationToken)
    {
        var paymentGroup = await _paymentGroupRepository.GetPaymentGroupById(request.Id);
        if (paymentGroup is null) return null;

        paymentGroup.Name = request.Name;
        paymentGroup.Description = request.Description;
        paymentGroup.Status = request.Status;
        paymentGroup.BrandId = _tenantContext.TenantId;

        var updated = await _paymentGroupRepository.UpdatePaymentGroup(paymentGroup);
        return _mapper.Map<PaymentGroupsDto>(updated);
    }
}
