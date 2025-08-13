using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IAccessGroupRepository : IGenericRepository<AccessGroup>
{
    Task<AccessGroup?> GetByGroupCodeAsync(Guid groupCode);
    Task<AccessGroup?> GetByGroupNameAsync(string groupName);
    Task<IEnumerable<AccessGroup>> GetActiveGroupsAsync();
    Task<IEnumerable<AccessGroup>> GetExpiringGroupsAsync(DateTime beforeDate);
    Task<IEnumerable<AccessGroup>> GetByCreatorAsync(string createdByNum);
    Task<bool> ExistsByGroupCodeAsync(Guid groupCode);
    Task<bool> ExistsByGroupNameAsync(string groupName);
}
