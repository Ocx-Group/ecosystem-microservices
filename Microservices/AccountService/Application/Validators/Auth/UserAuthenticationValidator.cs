using Ecosystem.AccountService.Application.Commands.Auth;
using FluentValidation;

namespace Ecosystem.AccountService.Application.Validators.Auth;

public class UserAuthenticationValidator : AbstractValidator<UserAuthenticationCommand>
{
    public UserAuthenticationValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("The User name field is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("The Password field is required");
    }
}
