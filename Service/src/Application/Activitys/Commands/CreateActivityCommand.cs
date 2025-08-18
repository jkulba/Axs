namespace Application.Activitys.Commands;

public record CreateActivityCommand(
    string ActivityCode,
    string ActivityName,
    string? Description,
    bool IsActive = true
);
