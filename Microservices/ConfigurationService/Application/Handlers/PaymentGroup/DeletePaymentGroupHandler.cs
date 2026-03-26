using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.PaymentGroup;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.PaymentGroup;

public class DeletePaymentGroupHandler : IRequestHandler<DeletePaymentGroupCommand, PaymentGroupsDto?>
{
    private readonly IPaymentGroupRepository _paymentGroupRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<DeletePaymentGroupHandler> _logger;

    public DeletePaymentGroupHandler(
        IPaymentGroupRepository paymentGroupRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<DeletePaymentGroupHandler> logger)
    {
        _paymentGroupRepository = paymentGroupRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaymentGroupsDto?> Handle(DeletePaymentGroupCommand request, CancellationToken cancellationToken)
    {
        var paymentGroup = await _paymentGroupRepository.GetPaymentGroupById(request.Id);
        if (paymentGroup is null) return null;

        var deleted = await _paymentGroupRepository.DeletePaymentGroup(paymentGroup);
        return _mapper.Map<PaymentGroupsDto>(deleted);
    }
}
