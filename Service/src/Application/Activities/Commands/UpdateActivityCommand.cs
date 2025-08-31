namespace Application.Activities.Commands;

public record UpdateActivityCommand(
    int Id,
    string ActivityCode,
    string ActivityName,
    string? Description,
    bool IsActive = true
);
