using Ecosystem.AccountService.Application.Commands.User;
using FluentValidation;

namespace Ecosystem.AccountService.Application.Validators.User;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("The UserName field is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("The Email field is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("The Password field is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");

        RuleFor(x => x.RolId)
            .GreaterThan(0).WithMessage("The RolId field is required");
    }
}
