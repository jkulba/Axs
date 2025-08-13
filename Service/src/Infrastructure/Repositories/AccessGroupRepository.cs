using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccessGroupRepository : IAccessGroupRepository
{
    private readonly AccessDbContext _context;

    public AccessGroupRepository(AccessDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<AccessGroup?> GetByIdAsync(int id)
    {
        return await _context.AccessGroups
            .FirstOrDefaultAsync(x => x.GroupId == id);
    }

    public async Task<IEnumerable<AccessGroup>> GetAllAsync()
    {
        return await _context.AccessGroups
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<AccessGroup> AddAsync(AccessGroup entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        entity.UtcCreatedAt = DateTime.UtcNow;
        
        _context.AccessGroups.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<AccessGroup> UpdateAsync(AccessGroup entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.AccessGroups.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.AccessGroups
            .AnyAsync(x => x.GroupId == id);
    }

    // Custom methods specific to AccessGroup
    public async Task<AccessGroup?> GetByGroupCodeAsync(Guid groupCode)
    {
        return await _context.AccessGroups
            .FirstOrDefaultAsync(x => x.GroupCode == groupCode);
    }

    public async Task<AccessGroup?> GetByGroupNameAsync(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            return null;

        return await _context.AccessGroups
            .FirstOrDefaultAsync(x => x.GroupName == groupName);
    }

    public async Task<IEnumerable<AccessGroup>> GetActiveGroupsAsync()
    {
        var currentDate = DateTime.UtcNow;
        return await _context.AccessGroups
            .Where(x => x.UtcExpirationDate == null || x.UtcExpirationDate > currentDate)
            .OrderBy(x => x.GroupName)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessGroup>> GetExpiringGroupsAsync(DateTime beforeDate)
    {
        return await _context.AccessGroups
            .Where(x => x.UtcExpirationDate != null && x.UtcExpirationDate <= beforeDate)
            .OrderBy(x => x.UtcExpirationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccessGroup>> GetByCreatorAsync(string createdByNum)
    {
        if (string.IsNullOrWhiteSpace(createdByNum))
            return Enumerable.Empty<AccessGroup>();

        return await _context.AccessGroups
            .Where(x => x.CreatedByNum == createdByNum)
            .OrderByDescending(x => x.UtcCreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByGroupCodeAsync(Guid groupCode)
    {
        return await _context.AccessGroups
            .AnyAsync(x => x.GroupCode == groupCode);
    }

    public async Task<bool> ExistsByGroupNameAsync(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            return false;

        return await _context.AccessGroups
            .AnyAsync(x => x.GroupName == groupName);
    }
}
