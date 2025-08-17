using Domain.Entities;

namespace Application.Interfaces;

public interface IAccessRequestRepository : IGenericRepository<AccessRequest>
{
    Task<AccessRequest?> GetByRequestCodeAsync(Guid requestCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<AccessRequest>> GetByJobNumberAsync(int jobNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<AccessRequest>> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<bool> ExistsByRequestCodeAsync(Guid requestCode, CancellationToken cancellationToken = default);
}
