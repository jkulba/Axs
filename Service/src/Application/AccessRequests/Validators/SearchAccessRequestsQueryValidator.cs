using FluentValidation;

namespace Application.AccessRequests.Validators;

public class SearchAccessRequestsQueryValidator : AbstractValidator<Queries.SearchAccessRequestsQuery>
{
    public SearchAccessRequestsQueryValidator()
    {
        RuleFor(x => x.SearchParameters)
            .NotNull()
            .WithMessage("Search parameters are required.");

        When(x => x.SearchParameters != null, () =>
        {
            RuleFor(x => x.SearchParameters.SearchTerm)
                .MaximumLength(500)
                .WithMessage("Search term cannot exceed 500 characters.");

            RuleFor(x => x.SearchParameters.UserName)
                .MaximumLength(100)
                .WithMessage("User name cannot exceed 100 characters.")
                .Matches(@"^[a-zA-Z0-9._@-]*$")
                .When(x => !string.IsNullOrEmpty(x.SearchParameters.UserName))
                .WithMessage("User name can only contain letters, numbers, dots, underscores, @ symbols, and hyphens.");

            RuleFor(x => x.SearchParameters.JobNumber)
                .GreaterThan(0)
                .When(x => x.SearchParameters.JobNumber.HasValue)
                .WithMessage("Job number must be a positive integer.");

            RuleFor(x => x.SearchParameters.CycleNumber)
                .GreaterThan(0)
                .When(x => x.SearchParameters.CycleNumber.HasValue)
                .WithMessage("Cycle number must be a positive integer.");

            RuleFor(x => x.SearchParameters.ActivityCode)
                .MaximumLength(50)
                .WithMessage("Activity code cannot exceed 50 characters.");

            RuleFor(x => x.SearchParameters.ApplicationName)
                .MaximumLength(255)
                .WithMessage("Application name cannot exceed 255 characters.");

            RuleFor(x => x.SearchParameters.Workstation)
                .MaximumLength(255)
                .WithMessage("Workstation cannot exceed 255 characters.");

            RuleFor(x => x.SearchParameters.CreatedFromDate)
                .LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
                .When(x => x.SearchParameters.CreatedFromDate.HasValue)
                .WithMessage("Created from date cannot be in the future.");

            RuleFor(x => x.SearchParameters.CreatedToDate)
                .LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
                .When(x => x.SearchParameters.CreatedToDate.HasValue)
                .WithMessage("Created to date cannot be in the future.");

            RuleFor(x => x.SearchParameters)
                .Must(HaveValidDateRange)
                .When(x => x.SearchParameters.CreatedFromDate.HasValue && x.SearchParameters.CreatedToDate.HasValue)
                .WithMessage("Created from date must be before or equal to created to date.");

            RuleFor(x => x.SearchParameters.PageNumber)
                .GreaterThan(0)
                .When(x => x.SearchParameters.PageNumber.HasValue)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.SearchParameters.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(1000)
                .When(x => x.SearchParameters.PageSize.HasValue)
                .WithMessage("Page size must be between 1 and 1000.");

            // If pagination is used, both PageNumber and PageSize must be provided
            RuleFor(x => x.SearchParameters.PageSize)
                .NotNull()
                .When(x => x.SearchParameters.PageNumber.HasValue)
                .WithMessage("Page size is required when page number is provided.");

            RuleFor(x => x.SearchParameters.PageNumber)
                .NotNull()
                .When(x => x.SearchParameters.PageSize.HasValue)
                .WithMessage("Page number is required when page size is provided.");
        });
    }

    private static bool HaveValidDateRange(Queries.AccessRequestSearchParameters searchParameters)
    {
        if (!searchParameters.CreatedFromDate.HasValue || !searchParameters.CreatedToDate.HasValue)
            return true;

        return searchParameters.CreatedFromDate.Value <= searchParameters.CreatedToDate.Value;
    }
}
