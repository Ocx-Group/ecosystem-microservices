using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.NotificationService.Application.DTOs;
using Ecosystem.NotificationService.Application.Queries.Template;
using Ecosystem.NotificationService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Template;

public class GetTemplateByKeyHandler : IRequestHandler<GetTemplateByKeyQuery, EmailTemplateDto?>
{
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetTemplateByKeyHandler(
        IEmailTemplateRepository templateRepository,
        ITenantContext tenantContext,
        IMapper mapper)
    {
        _templateRepository = templateRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<EmailTemplateDto?> Handle(GetTemplateByKeyQuery request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByKeyAndBrandAsync(request.TemplateKey, _tenantContext.TenantId);
        return template is null ? null : _mapper.Map<EmailTemplateDto>(template);
    }
}
