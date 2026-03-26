using Ecosystem.AccountService.Application.Commands.Affiliate;
using FluentValidation;

namespace Ecosystem.AccountService.Application.Validators.Affiliate;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.VerificationCode)
            .NotEmpty().WithMessage("The VerificationCode field is required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("The Password field is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
    }
}
