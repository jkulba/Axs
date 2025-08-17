using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccessRequestRepository : GenericRepository<AccessRequest>, IAccessRequestRepository
{
    public AccessRequestRepository(AccessDbContext context) : base(context)
    {
    }

    public async Task<AccessRequest?> GetByRequestCodeAsync(Guid requestCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(ar => ar.RequestCode == requestCode, cancellationToken);
    }

    public async Task<IEnumerable<AccessRequest>> GetByJobNumberAsync(int jobNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ar => ar.JobNumber == jobNumber)
            .OrderByDescending(ar => ar.UtcCreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AccessRequest>> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ar => ar.UserName == userName)
            .OrderByDescending(ar => ar.UtcCreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByRequestCodeAsync(Guid requestCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(ar => ar.RequestCode == requestCode, cancellationToken);
    }
}
