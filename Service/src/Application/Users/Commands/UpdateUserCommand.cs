namespace Application.Users.Commands;

public record UpdateUserCommand(
    int Id,
    string GraphId,
    string UserId,
    string DisplayName,
    string PrincipalName,
    bool IsEnabled = true
);