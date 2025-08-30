using Application.Users.Commands;
using FluentValidation;

namespace Application.Users.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(x => x.GraphId)
            .NotEmpty()
            .WithMessage("Graph ID is required.")
            .MaximumLength(255)
            .WithMessage("Graph ID cannot exceed 255 characters.")
            .Matches(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")
            .WithMessage("Graph ID must be a valid GUID format.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .MaximumLength(255)
            .WithMessage("User ID cannot exceed 255 characters.")
            .Matches(@"^[a-zA-Z0-9._-]+$")
            .WithMessage("User ID can only contain letters, numbers, dots, underscores, and hyphens.");

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .WithMessage("Display name is required.")
            .MaximumLength(255)
            .WithMessage("Display name cannot exceed 255 characters.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Display name cannot be only whitespace.");

        RuleFor(x => x.PrincipalName)
            .NotEmpty()
            .WithMessage("Principal name is required.")
            .MaximumLength(255)
            .WithMessage("Principal name cannot exceed 255 characters.")
            .EmailAddress()
            .WithMessage("Principal name must be a valid email address format.");

        RuleFor(x => x.IsEnabled)
            .NotNull()
            .WithMessage("IsEnabled field is required.");
    }
}
