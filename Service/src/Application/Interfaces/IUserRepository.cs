using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
