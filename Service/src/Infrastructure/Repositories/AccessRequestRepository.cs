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

    public async Task<AccessRequest?> GetByRequestCodeAsync(Guid requestCode)
    {
        return await _dbSet
            .FirstOrDefaultAsync(ar => ar.RequestCode == requestCode);
    }

    public async Task<IEnumerable<AccessRequest>> GetByJobNumberAsync(int jobNumber)
    {
        return await _dbSet
            .Where(ar => ar.JobNumber == jobNumber)
            .OrderByDescending(ar => ar.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequest>> GetByUserNameAsync(string userName)
    {
        return await _dbSet
            .Where(ar => ar.UserName == userName)
            .OrderByDescending(ar => ar.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByRequestCodeAsync(Guid requestCode)
    {
        return await _dbSet
            .AnyAsync(ar => ar.RequestCode == requestCode);
    }
}
