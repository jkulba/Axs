using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Services;

public interface IAuthorizationService
{
    Task<bool> IsUserAuthorized(string userId, int jobNumber, int activityId);
}

public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationRepository _authRepository;

    public AuthorizationService(IAuthorizationRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<bool> IsUserAuthorized(string userId, int jobNumber, int activityId)
    {
        var result = await _authRepository.CheckUserAuthorization(userId, jobNumber, activityId);

        // You could add logging here
        // Log.Information("Authorization check for user {UserId}, job {JobNumber}, activity {ActivityId}: {Result} ({Source})",
        //     userId, jobNumber, activityId, result.IsAuthorized, result.Source);

        return result.IsAuthorized;
    }
}
