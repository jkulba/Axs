using Application.Activities.Commands;
using FluentValidation;

namespace Application.Activities.Validators;

public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
{
    public UpdateActivityCommandValidator()
    {
        RuleFor(x => x.ActivityId)
            .GreaterThan(0)
            .WithMessage("Activity ID must be greater than 0.");

        RuleFor(x => x.ActivityCode)
            .NotEmpty()
            .WithMessage("Activity code is required.")
            .MaximumLength(50)
            .WithMessage("Activity code cannot exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9._-]+$")
            .WithMessage("Activity code can only contain letters, numbers, dots, underscores, and hyphens.");

        RuleFor(x => x.ActivityName)
            .NotEmpty()
            .WithMessage("Activity name is required.")
            .MaximumLength(255)
            .WithMessage("Activity name cannot exceed 255 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
