using AutoMapper;
using Ecosystem.NotificationService.Application.Commands.Template;
using Ecosystem.NotificationService.Application.DTOs;
using Ecosystem.NotificationService.Domain.Interfaces;
using Ecosystem.NotificationService.Domain.Models;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Template;

public class CreateTemplateHandler : IRequestHandler<CreateTemplateCommand, EmailTemplateDto>
{
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly IMapper _mapper;

    public CreateTemplateHandler(IEmailTemplateRepository templateRepository, IMapper mapper)
    {
        _templateRepository = templateRepository;
        _mapper = mapper;
    }

    public async Task<EmailTemplateDto> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var existing = await _templateRepository.GetByKeyAndBrandAsync(request.TemplateKey, request.BrandId);
        if (existing is not null)
            throw new InvalidOperationException(
                $"Template '{request.TemplateKey}' already exists for brand {request.BrandId}");

        var template = new EmailTemplate
        {
            TemplateKey = request.TemplateKey,
            BrandId = request.BrandId,
            Subject = request.Subject,
            HtmlBody = request.HtmlBody,
            Placeholders = request.Placeholders
        };

        var created = await _templateRepository.CreateAsync(template);
        return _mapper.Map<EmailTemplateDto>(created);
    }
}
