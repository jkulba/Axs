using Api.Contracts;
using FluentValidation;

namespace Api.Validators;

public class VerifyAccessRequestValidator : AbstractValidator<VerifyAccessRequest>
{
    public VerifyAccessRequestValidator()
    {
        RuleFor(x => x.ProfileUserName)
            .NotEmpty()
            .WithMessage("ProfileUserName is required.")
            .MaximumLength(100)
            .WithMessage("ProfileUserName cannot exceed 100 characters.");

        RuleFor(x => x.JobNumber)
            .GreaterThan(0)
            .WithMessage("JobNumber must be greater than 0.");

        RuleFor(x => x.CycleNumber)
            .GreaterThanOrEqualTo(0)
            .WithMessage("CycleNumber must be greater than or equal to 0.");

        RuleFor(x => x.Workstation)
            .NotEmpty()
            .WithMessage("Workstation is required.")
            .MaximumLength(50)
            .WithMessage("Workstation cannot exceed 50 characters.");

        RuleFor(x => x.ApplicationName)
            .NotEmpty()
            .WithMessage("ApplicationName is required.")
            .MaximumLength(100)
            .WithMessage("ApplicationName cannot exceed 100 characters.");
    }
}
