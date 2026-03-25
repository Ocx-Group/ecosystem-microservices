using Ecosystem.AccountService.Application.Commands.Ticket;
using FluentValidation;

namespace Ecosystem.AccountService.Application.Validators.Ticket;

public class CreateTicketValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketValidator()
    {
        RuleFor(x => x.AffiliateId).GreaterThan(0).WithMessage("AffiliateId is required.");
        RuleFor(x => x.Subject).NotEmpty().WithMessage("Subject is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
    }
}
