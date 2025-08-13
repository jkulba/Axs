using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IAccessRequestHistoryRepository : IGenericRepository<AccessRequestHistory>
{
    Task<AccessRequestHistory?> GetByRequestCodeAsync(Guid requestCode);
    Task<IEnumerable<AccessRequestHistory>> GetByRequestIdAsync(int requestId);
    Task<IEnumerable<AccessRequestHistory>> GetByEmployeeNumAsync(string employeeNum);
    Task<IEnumerable<AccessRequestHistory>> GetByOperationAsync(int operation);
    Task<IEnumerable<AccessRequestHistory>> GetByApprovalStatusAsync(int approvalStatus);
    Task<IEnumerable<AccessRequestHistory>> GetByJobNumberAsync(int jobNumber);
    Task<IEnumerable<AccessRequestHistory>> GetByApproverAsync(string approverNum);
    Task<IEnumerable<AccessRequestHistory>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);
    Task<IEnumerable<AccessRequestHistory>> GetRecentHistoryAsync(int days = 30);
    Task<IEnumerable<AccessRequestHistory>> GetExpiringAccessAsync(DateTime beforeDate);
    Task<bool> ExistsByRequestCodeAsync(Guid requestCode);
}
