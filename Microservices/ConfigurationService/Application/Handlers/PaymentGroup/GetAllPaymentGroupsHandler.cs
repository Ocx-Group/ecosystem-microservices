using AutoMapper;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.PaymentGroup;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.PaymentGroup;

public class GetAllPaymentGroupsHandler : IRequestHandler<GetAllPaymentGroupsQuery, ICollection<PaymentGroupsDto>>
{
    private readonly IPaymentGroupRepository _paymentGroupRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllPaymentGroupsHandler> _logger;

    public GetAllPaymentGroupsHandler(
        IPaymentGroupRepository paymentGroupRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetAllPaymentGroupsHandler> logger)
    {
        _paymentGroupRepository = paymentGroupRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<PaymentGroupsDto>> Handle(GetAllPaymentGroupsQuery request, CancellationToken cancellationToken)
    {
        var paymentGroups = await _paymentGroupRepository.GetAllPaymentGroups(_tenantContext.TenantId);
        return _mapper.Map<ICollection<PaymentGroupsDto>>(paymentGroups);
    }
}
