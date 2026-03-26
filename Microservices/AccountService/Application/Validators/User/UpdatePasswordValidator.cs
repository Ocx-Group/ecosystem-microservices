using Ecosystem.AccountService.Application.Commands.User;
using FluentValidation;

namespace Ecosystem.AccountService.Application.Validators.User;

public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("The Id field is required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("The current Password field is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("The NewPassword field is required")
            .MinimumLength(6).WithMessage("New password must be at least 6 characters");
    }
}
