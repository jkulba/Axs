namespace Application.Activities.Commands;

public record CreateActivityCommand(
    string ActivityCode,
    string ActivityName,
    string? Description,
    bool IsActive = true
);
