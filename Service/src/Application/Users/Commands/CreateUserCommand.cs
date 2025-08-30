namespace Application.Users.Commands;

public record CreateUserCommand(
    string GraphId,
    string UserId,
    string DisplayName,
    string PrincipalName,
    bool IsEnabled = true
);