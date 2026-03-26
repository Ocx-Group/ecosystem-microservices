using Ecosystem.NotificationService.Application.Commands.Brand;
using FluentValidation;

namespace Ecosystem.NotificationService.Application.Validators.Brand;

public class CreateBrandConfigurationValidator : AbstractValidator<CreateBrandConfigurationCommand>
{
    public CreateBrandConfigurationValidator()
    {
        RuleFor(x => x.BrandId)
            .GreaterThan(0).WithMessage("BrandId must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.SenderName)
            .NotEmpty().WithMessage("SenderName is required");

        RuleFor(x => x.SenderEmail)
            .NotEmpty().WithMessage("SenderEmail is required")
            .EmailAddress().WithMessage("SenderEmail must be a valid email address");
    }
}
