namespace Application.AccessRequests.Commands;

public record CreateAccessRequestCommand(
    Guid? RequestCode,
    string? UserName,
    int JobNumber,
    int CycleNumber,
    string ActivityCode,
    string Workstation,
    string? ApplicationName,
    DateTime? UtcCreatedAt
);