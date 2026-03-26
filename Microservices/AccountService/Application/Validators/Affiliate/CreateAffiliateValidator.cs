using Ecosystem.AccountService.Application.Commands.Affiliate;
using FluentValidation;

namespace Ecosystem.AccountService.Application.Validators.Affiliate;

public class CreateAffiliateValidator : AbstractValidator<CreateAffiliateCommand>
{
    public CreateAffiliateValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("The UserName field is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("The Email field is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("The Password field is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
    }
}
