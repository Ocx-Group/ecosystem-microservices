using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.Domain.Core.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Consumers;

public class RevertActivationDateConsumer : IConsumer<RevertActivationDateEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<RevertActivationDateConsumer> _logger;

    public RevertActivationDateConsumer(IMediator mediator, ILogger<RevertActivationDateConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<RevertActivationDateEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Received RevertActivationDateEvent: affiliateId={AffiliateId}, brandId={BrandId}",
            message.AffiliateId, message.BrandId);

        var result = await _mediator.Send(new RevertActivationCommand(message.AffiliateId));

        if (result is null)
            _logger.LogWarning("Affiliate {AffiliateId} not found for activation revert", message.AffiliateId);
        else
            _logger.LogInformation("Activation reverted for affiliate {AffiliateId}", message.AffiliateId);
    }
}
