using AutoMapper;
using Ecosystem.NotificationService.Application.Commands.Template;
using Ecosystem.NotificationService.Application.DTOs;
using Ecosystem.NotificationService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Template;

public class UpdateTemplateHandler : IRequestHandler<UpdateTemplateCommand, EmailTemplateDto>
{
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly IMapper _mapper;

    public UpdateTemplateHandler(IEmailTemplateRepository templateRepository, IMapper mapper)
    {
        _templateRepository = templateRepository;
        _mapper = mapper;
    }

    public async Task<EmailTemplateDto> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Template with id '{request.Id}' not found");

        if (request.TemplateKey is not null) template.TemplateKey = request.TemplateKey;
        if (request.BrandId.HasValue) template.BrandId = request.BrandId.Value;
        if (request.Subject is not null) template.Subject = request.Subject;
        if (request.HtmlBody is not null) template.HtmlBody = request.HtmlBody;
        if (request.Placeholders is not null) template.Placeholders = request.Placeholders;
        if (request.IsActive.HasValue) template.IsActive = request.IsActive.Value;

        template.Version++;

        var updated = await _templateRepository.UpdateAsync(template);
        return _mapper.Map<EmailTemplateDto>(updated);
    }
}
