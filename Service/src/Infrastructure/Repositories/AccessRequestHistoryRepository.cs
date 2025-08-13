using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccessRequestHistoryRepository : IAccessRequestHistoryRepository
{
    private readonly AccessDbContext _context;

    public AccessRequestHistoryRepository(AccessDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<AccessRequestHistory?> GetByIdAsync(int id)
    {
        return await _context.AccessRequestHistories
            .FirstOrDefaultAsync(x => x.AccessRequestHistoryId == id);
    }

    public async Task<IEnumerable<AccessRequestHistory>> GetAllAsync()
    {
        return await _context.AccessRequestHistories
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<AccessRequestHistory> AddAsync(AccessRequestHistory entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        entity.UtcCreatedAt = DateTime.UtcNow;
        
        _context.AccessRequestHistories.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<AccessRequestHistory> UpdateAsync(AccessRequestHistory entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        entity.UtcUpdatedAt = DateTime.UtcNow;
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.AccessRequestHistories.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.AccessRequestHistories
            .AnyAsync(x => x.AccessRequestHistoryId == id);
    }

    // Custom methods specific to AccessRequestHistory
    public async Task<AccessRequestHistory?> GetByRequestCodeAsync(Guid requestCode)
    {
        return await _context.AccessRequestHistories
            .Where(x => x.RequestCode == requestCode)
            .OrderByDescending(x => x.UtcCreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<AccessRequestHistory>> GetByRequestIdAsync(int requestId)
    {
        return await _context.AccessRequestHistories
            .Where(x => x.RequestId == requestId)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequestHistory>> GetByEmployeeNumAsync(string employeeNum)
    {
        if (string.IsNullOrWhiteSpace(employeeNum))
            return Enumerable.Empty<AccessRequestHistory>();

        return await _context.AccessRequestHistories
            .Where(x => x.EmployeeNum == employeeNum)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequestHistory>> GetByOperationAsync(int operation)
    {
        return await _context.AccessRequestHistories
            .Where(x => x.Operation == operation)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequestHistory>> GetByApprovalStatusAsync(int approvalStatus)
    {
        return await _context.AccessRequestHistories
            .Where(x => x.ApprovalStatus == approvalStatus)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequestHistory>> GetByJobNumberAsync(int jobNumber)
    {
        return await _context.AccessRequestHistories
            .Where(x => x.JobNumber == jobNumber)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequestHistory>> GetByApproverAsync(string approverNum)
    {
        if (string.IsNullOrWhiteSpace(approverNum))
            return Enumerable.Empty<AccessRequestHistory>();

        return await _context.AccessRequestHistories
            .Where(x => x.ApproverNum == approverNum)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequestHistory>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
    {
        return await _context.AccessRequestHistories
            .Where(x => x.UtcCreatedAt >= fromDate && x.UtcCreatedAt <= toDate)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequestHistory>> GetRecentHistoryAsync(int days = 30)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);
        return await _context.AccessRequestHistories
            .Where(x => x.UtcCreatedAt >= cutoffDate)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequestHistory>> GetExpiringAccessAsync(DateTime beforeDate)
    {
        return await _context.AccessRequestHistories
            .Where(x => x.AccessExpiresAt != null && x.AccessExpiresAt <= beforeDate)
            .OrderBy(x => x.AccessExpiresAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByRequestCodeAsync(Guid requestCode)
    {
        return await _context.AccessRequestHistories
            .AnyAsync(x => x.RequestCode == requestCode);
    }
}
