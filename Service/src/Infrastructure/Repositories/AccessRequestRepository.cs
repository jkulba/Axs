using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccessRequestRepository : IAccessRequestRepository
{
    private readonly AccessDbContext _context;

    public AccessRequestRepository(AccessDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<AccessRequest?> GetByIdAsync(int id)
    {
        return await _context.AccessRequests
            .FirstOrDefaultAsync(x => x.RequestId == id);
    }

    public async Task<IEnumerable<AccessRequest>> GetAllAsync()
    {
        return await _context.AccessRequests
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<AccessRequest> AddAsync(AccessRequest entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.AccessRequests.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<AccessRequest> UpdateAsync(AccessRequest entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.AccessRequests.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.AccessRequests.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.AccessRequests
            .AnyAsync(x => x.RequestId == id);
    }

    // Domain-specific methods
    public async Task<AccessRequest?> GetByRequestCodeAsync(Guid requestCode)
    {
        return await _context.AccessRequests
            .FirstOrDefaultAsync(x => x.RequestCode == requestCode);
    }

    public async Task<AccessRequest?> GetByEmployeeNumAsync(string employeeNum)
    {
        if (string.IsNullOrWhiteSpace(employeeNum))
            throw new ArgumentException("Employee number cannot be null or empty", nameof(employeeNum));

        return await _context.AccessRequests
            .FirstOrDefaultAsync(x => x.EmployeeNum == employeeNum);
    }

    public async Task<IEnumerable<AccessRequest>> GetByApprovalStatusAsync(int approvalStatus)
    {
        return await _context.AccessRequests
            .Where(x => x.ApprovalStatus == approvalStatus)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequest>> GetByJobNumberAsync(int jobNumber)
    {
        return await _context.AccessRequests
            .Where(x => x.JobNumber == jobNumber)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequest>> GetExpiringRequestsAsync(DateTime beforeDate)
    {
        return await _context.AccessRequests
            .Where(x => x.AccessExpiresAt.HasValue && x.AccessExpiresAt <= beforeDate)
            .OrderBy(x => x.AccessExpiresAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessRequest>> GetRequestsByApproverAsync(string approverNum)
    {
        if (string.IsNullOrWhiteSpace(approverNum))
            throw new ArgumentException("Approver number cannot be null or empty", nameof(approverNum));

        return await _context.AccessRequests
            .Where(x => x.ApproverNum == approverNum)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByEmployeeNumAsync(string employeeNum)
    {
        if (string.IsNullOrWhiteSpace(employeeNum))
            return false;

        return await _context.AccessRequests
            .AnyAsync(x => x.EmployeeNum == employeeNum);
    }

    public async Task<bool> ExistsByRequestCodeAsync(Guid requestCode)
    {
        return await _context.AccessRequests
            .AnyAsync(x => x.RequestCode == requestCode);
    }
}
