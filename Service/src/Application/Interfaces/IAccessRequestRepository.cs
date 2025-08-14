using Domain.Entities;

namespace Application.Interfaces;

public interface IAccessRequestRepository : IGenericRepository<AccessRequest>
{
    Task<AccessRequest?> GetByRequestCodeAsync(Guid requestCode);
    Task<IEnumerable<AccessRequest>> GetByJobNumberAsync(int jobNumber);
    Task<IEnumerable<AccessRequest>> GetByUserNameAsync(string userName);
    Task<bool> ExistsByRequestCodeAsync(Guid requestCode);
}
