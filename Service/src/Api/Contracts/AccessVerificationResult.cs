namespace Api.Contracts;

public record AccessVerificationResult
{
    public required bool IsAccessGranted { get; set; }
    public string? Message { get; set; }
    public DateTime VerificationTime { get; set; } = DateTime.UtcNow;
}
