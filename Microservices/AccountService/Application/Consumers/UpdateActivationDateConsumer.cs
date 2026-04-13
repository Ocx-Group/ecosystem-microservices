using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.Domain.Core.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Consumers;

public class UpdateActivationDateConsumer : IConsumer<UpdateActivationDateEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateActivationDateConsumer> _logger;

    public UpdateActivationDateConsumer(IMediator mediator, ILogger<UpdateActivationDateConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UpdateActivationDateEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Received UpdateActivationDateEvent: affiliateId={AffiliateId}, brandId={BrandId}",
            message.AffiliateId, message.BrandId);

        var result = await _mediator.Send(new UpdateActivationDateCommand(message.AffiliateId));

        if (result is null)
            _logger.LogWarning("Affiliate {AffiliateId} not found for activation date update", message.AffiliateId);
        else
            _logger.LogInformation("Activation date updated for affiliate {AffiliateId}", message.AffiliateId);
    }
}
