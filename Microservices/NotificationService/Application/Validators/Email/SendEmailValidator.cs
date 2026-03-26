using Ecosystem.NotificationService.Application.Commands.Email;
using FluentValidation;

namespace Ecosystem.NotificationService.Application.Validators.Email;

public class SendEmailValidator : AbstractValidator<SendEmailCommand>
{
    public SendEmailValidator()
    {
        RuleFor(x => x.TemplateKey)
            .NotEmpty().WithMessage("TemplateKey is required");

        RuleFor(x => x.BrandId)
            .GreaterThan(0).WithMessage("BrandId must be greater than 0");

        RuleFor(x => x.ToEmail)
            .NotEmpty().WithMessage("ToEmail is required")
            .EmailAddress().WithMessage("ToEmail must be a valid email address");

        RuleFor(x => x.ToName)
            .NotEmpty().WithMessage("ToName is required");

        RuleFor(x => x.Placeholders)
            .NotNull().WithMessage("Placeholders is required");
    }
}
