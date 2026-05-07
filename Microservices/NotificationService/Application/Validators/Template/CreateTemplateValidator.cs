using Ecosystem.NotificationService.Application.Commands.Template;
using FluentValidation;

namespace Ecosystem.NotificationService.Application.Validators.Template;

public class CreateTemplateValidator : AbstractValidator<CreateTemplateCommand>
{
    public CreateTemplateValidator()
    {
        RuleFor(x => x.TemplateKey)
            .NotEmpty().WithMessage("TemplateKey is required")
            .MaximumLength(100).WithMessage("TemplateKey must not exceed 100 characters");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required")
            .MaximumLength(200).WithMessage("Subject must not exceed 200 characters");

        RuleFor(x => x.HtmlBody)
            .NotEmpty().WithMessage("HtmlBody is required");
    }
}
