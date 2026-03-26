using Ecosystem.AccountService.Application.Commands.AffiliateBtc;
using FluentValidation;

namespace Ecosystem.AccountService.Application.Validators.AffiliateBtc;

public class CreateAffiliateBtcValidator : AbstractValidator<CreateAffiliateBtcCommand>
{
    public CreateAffiliateBtcValidator()
    {
        RuleFor(x => x.AffiliateId)
            .GreaterThan(0).WithMessage("AffiliateId must be greater than 0");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");

        RuleFor(x => x.VerificationCode)
            .NotEmpty().WithMessage("VerificationCode is required");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Trc20Address) || !string.IsNullOrEmpty(x.BscAddress))
            .WithMessage("At least one address (Trc20 or Bsc) must be provided");
    }
}
