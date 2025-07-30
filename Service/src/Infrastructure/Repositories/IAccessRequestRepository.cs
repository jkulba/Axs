using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IAccessRequestRepository : IGenericRepository<AccessRequest>
{
    Task<AccessRequest?> GetByRequestCodeAsync(Guid requestCode);
    Task<AccessRequest?> GetByEmployeeNumAsync(string employeeNum);
    Task<IEnumerable<AccessRequest>> GetByApprovalStatusAsync(int approvalStatus);
    Task<IEnumerable<AccessRequest>> GetByJobNumberAsync(int jobNumber);
    Task<IEnumerable<AccessRequest>> GetExpiringRequestsAsync(DateTime beforeDate);
    Task<IEnumerable<AccessRequest>> GetRequestsByApproverAsync(string approverNum);
    Task<bool> ExistsByEmployeeNumAsync(string employeeNum);
    Task<bool> ExistsByRequestCodeAsync(Guid requestCode);
}
