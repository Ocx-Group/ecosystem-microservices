using AutoMapper;
using Ecosystem.NotificationService.Application.DTOs;
using Ecosystem.NotificationService.Application.Queries.Template;
using Ecosystem.NotificationService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Template;

public class GetAllTemplatesHandler : IRequestHandler<GetAllTemplatesQuery, ICollection<EmailTemplateDto>>
{
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly IMapper _mapper;

    public GetAllTemplatesHandler(IEmailTemplateRepository templateRepository, IMapper mapper)
    {
        _templateRepository = templateRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<EmailTemplateDto>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken)
    {
        var templates = request.BrandId.HasValue
            ? await _templateRepository.GetByBrandAsync(request.BrandId.Value)
            : await _templateRepository.GetAllAsync();

        return _mapper.Map<ICollection<EmailTemplateDto>>(templates);
    }
}
