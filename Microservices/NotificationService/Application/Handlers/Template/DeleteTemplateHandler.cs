using Ecosystem.NotificationService.Application.Commands.Template;
using Ecosystem.NotificationService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Template;

public class DeleteTemplateHandler : IRequestHandler<DeleteTemplateCommand, bool>
{
    private readonly IEmailTemplateRepository _templateRepository;

    public DeleteTemplateHandler(IEmailTemplateRepository templateRepository)
        => _templateRepository = templateRepository;

    public async Task<bool> Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
        => await _templateRepository.DeleteAsync(request.Id);
}
