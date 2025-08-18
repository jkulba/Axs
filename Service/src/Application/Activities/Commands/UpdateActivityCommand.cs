namespace Application.Activities.Commands;

public record UpdateActivityCommand(
    int ActivityId,
    string ActivityCode,
    string ActivityName,
    string? Description,
    bool IsActive = true
);
