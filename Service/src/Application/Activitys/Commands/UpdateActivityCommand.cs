namespace Application.Activitys.Commands;

public record UpdateActivityCommand(
    int ActivityId,
    string ActivityCode,
    string ActivityName,
    string? Description,
    bool IsActive = true
);