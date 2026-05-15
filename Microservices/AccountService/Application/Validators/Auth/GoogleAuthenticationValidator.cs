using Ecosystem.AccountService.Application.Commands.Auth;
using FluentValidation;

namespace Ecosystem.AccountService.Application.Validators.Auth;

public class GoogleAuthenticationValidator : AbstractValidator<GoogleAuthenticationCommand>
{
    public GoogleAuthenticationValidator()
    {
        RuleFor(x => x.IdToken)
            .NotEmpty()
            .WithMessage("The Google token is required");
    }
}
