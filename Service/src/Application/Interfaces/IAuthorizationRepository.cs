namespace Application.Interfaces;

public interface IAuthorizationRepository
{
    Task<AuthorizationResult> CheckUserAuthorization(string userId, int jobNumber, int activityId);
}

public class AuthorizationResult
{
    public bool IsAuthorized { get; set; }
    public string Source { get; set; } = null!;
}
