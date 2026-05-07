using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.NotificationService.Application.DTOs;
using Ecosystem.NotificationService.Application.Queries.Template;
using Ecosystem.NotificationService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Template;

public class GetAllTemplatesHandler : IRequestHandler<GetAllTemplatesQuery, ICollection<EmailTemplateDto>>
{
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetAllTemplatesHandler(
        IEmailTemplateRepository templateRepository,
        ITenantContext tenantContext,
        IMapper mapper)
    {
        _templateRepository = templateRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<ICollection<EmailTemplateDto>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken)
    {
        var templates = await _templateRepository.GetByBrandAsync(_tenantContext.TenantId);
        return _mapper.Map<ICollection<EmailTemplateDto>>(templates);
    }
}
