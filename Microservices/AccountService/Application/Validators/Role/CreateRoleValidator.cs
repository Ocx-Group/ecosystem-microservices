using Ecosystem.AccountService.Application.Commands.Role;
using FluentValidation;

namespace Ecosystem.AccountService.Application.Validators.Role;

public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name field is required");
    }
}
