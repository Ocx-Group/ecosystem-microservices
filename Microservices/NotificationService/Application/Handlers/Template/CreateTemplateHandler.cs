using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.NotificationService.Application.Commands.Template;
using Ecosystem.NotificationService.Application.DTOs;
using Ecosystem.NotificationService.Domain.Interfaces;
using Ecosystem.NotificationService.Domain.Models;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Template;

public class CreateTemplateHandler : IRequestHandler<CreateTemplateCommand, EmailTemplateDto>
{
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public CreateTemplateHandler(
        IEmailTemplateRepository templateRepository,
        ITenantContext tenantContext,
        IMapper mapper)
    {
        _templateRepository = templateRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<EmailTemplateDto> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var existing = await _templateRepository.GetByKeyAndBrandAsync(request.TemplateKey, brandId);
        if (existing is not null)
            throw new InvalidOperationException(
                $"Template '{request.TemplateKey}' already exists for brand {brandId}");

        var template = new EmailTemplate
        {
            TemplateKey = request.TemplateKey,
            BrandId = brandId,
            Subject = request.Subject,
            HtmlBody = request.HtmlBody,
            Placeholders = request.Placeholders,
        };

        var created = await _templateRepository.CreateAsync(template);
        return _mapper.Map<EmailTemplateDto>(created);
    }
}
