using Refit;
using System.Text.Json;

namespace Client.Services;

public interface IUserApi
{

    // Create a new user
    [Post("/api/users")]
    Task<User> CreateUserAsync([Body] User newUser, CancellationToken cancellationToken = default);

    // Get all users
    [Get("/api/users")]
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);

    // Update an existing user
    [Put("/api/users/{id}")]
    Task<User> UpdateUserAsync(int id, [Body] User updatedUser, CancellationToken cancellationToken = default);

    // Delete a user
    [Delete("/api/users/{id}")]
    Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);

    // Get a user by Id
    [Get("/api/users/{id}")]
    Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);

    // Get a user by UserId
    [Get("/api/users/userid/{userId}")]
    Task<User> GetUserByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}

public class UserSearchParameters
{
    public string? UserId { get; set; }
    public string? PrincipalName { get; set; }
    public bool? IsEnabled { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}

public record User(
    int Id,
    string GraphId,
    string UserId,
    string DisplayName,
    string PrincipalName,
    bool IsEnabled,
    DateTime LastUpdated,
    DateTime UtcCreatedAt,
    string? CreatedByNum,
    DateTime? UtcUpdatedAt,
    string? UpdatedByNum
);


