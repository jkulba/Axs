using Application.Activities.Commands;
using FluentValidation;

namespace Application.Activities.Validators;

public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
{
    public CreateActivityCommandValidator()
    {
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
