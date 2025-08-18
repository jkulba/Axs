namespace Application.Authorization.Commands;

public record VerifyAccessCommand(
    string ProfileUserName,
    int JobNumber,
    int CycleNumber,
    string Workstation,
    string ApplicationName,
    string? ActivityCode
);
