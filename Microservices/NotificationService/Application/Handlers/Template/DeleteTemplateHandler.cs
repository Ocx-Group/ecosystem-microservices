using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.NotificationService.Application.Commands.Template;
using Ecosystem.NotificationService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Template;

public class DeleteTemplateHandler : IRequestHandler<DeleteTemplateCommand, bool>
{
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly ITenantContext _tenantContext;

    public DeleteTemplateHandler(
        IEmailTemplateRepository templateRepository,
        ITenantContext tenantContext)
    {
        _templateRepository = templateRepository;
        _tenantContext = tenantContext;
    }

    public async Task<bool> Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByIdAsync(request.Id);
        if (template is null) return false;

        if (template.BrandId != _tenantContext.TenantId)
            throw new UnauthorizedAccessException(
                $"Template {request.Id} does not belong to the current tenant");

        return await _templateRepository.DeleteAsync(request.Id);
    }
}
