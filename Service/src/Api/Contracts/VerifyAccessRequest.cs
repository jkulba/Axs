namespace Api.Contracts;

public record VerifyAccessRequest
{
    public required string ProfileUserName { get; set; }

    public required int JobNumber { get; set; }

    public int CycleNumber { get; set; }

    public required string Workstation { get; set; }

    public required string ApplicationName { get; set; }

    public string? ActivityCode { get; set; }

}
