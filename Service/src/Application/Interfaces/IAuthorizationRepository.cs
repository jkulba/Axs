namespace Application.Interfaces;

public interface IAuthorizationRepository
{
    Task<AuthorizationResult> CheckUserAuthorization(string userId, int jobNumber, int activityId, CancellationToken cancellationToken = default);
}

public class AuthorizationResult
{
    public bool IsAuthorized { get; set; }
    public string Source { get; set; } = null!;
}
