using Application.AccessRequests.Queries;
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
            .AsNoTracking()
            .FirstOrDefaultAsync(ar => ar.RequestCode == requestCode, cancellationToken);
    }

    public async Task<IEnumerable<AccessRequest>> GetByJobNumberAsync(int jobNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(ar => ar.JobNumber == jobNumber)
            .OrderByDescending(ar => ar.UtcCreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AccessRequest>> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(ar => ar.UserName == userName)
            .OrderByDescending(ar => ar.UtcCreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByRequestCodeAsync(Guid requestCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(ar => ar.RequestCode == requestCode, cancellationToken);
    }

    public async Task<IEnumerable<AccessRequest>> SearchAsync(AccessRequestSearchParameters searchParameters, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        // Apply filters based on provided search parameters
        if (!string.IsNullOrWhiteSpace(searchParameters.SearchTerm))
        {
            var searchTerm = searchParameters.SearchTerm.ToLower();
            query = query.Where(ar =>
                ar.UserName.ToLower().Contains(searchTerm) ||
                (ar.ActivityCode != null && ar.ActivityCode.ToLower().Contains(searchTerm)) ||
                (ar.ApplicationName != null && ar.ApplicationName.ToLower().Contains(searchTerm)) ||
                (ar.Workstation != null && ar.Workstation.ToLower().Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(searchParameters.UserName))
        {
            query = query.Where(ar => ar.UserName.ToLower().Contains(searchParameters.UserName.ToLower()));
        }

        if (searchParameters.JobNumber.HasValue)
        {
            query = query.Where(ar => ar.JobNumber == searchParameters.JobNumber.Value);
        }

        if (searchParameters.CycleNumber.HasValue)
        {
            query = query.Where(ar => ar.CycleNumber == searchParameters.CycleNumber.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchParameters.ActivityCode))
        {
            query = query.Where(ar => ar.ActivityCode != null && ar.ActivityCode.ToLower().Contains(searchParameters.ActivityCode.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(searchParameters.ApplicationName))
        {
            query = query.Where(ar => ar.ApplicationName != null && ar.ApplicationName.ToLower().Contains(searchParameters.ApplicationName.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(searchParameters.Workstation))
        {
            query = query.Where(ar => ar.Workstation != null && ar.Workstation.ToLower().Contains(searchParameters.Workstation.ToLower()));
        }

        if (searchParameters.CreatedFromDate.HasValue)
        {
            query = query.Where(ar => ar.UtcCreatedAt >= searchParameters.CreatedFromDate.Value);
        }

        if (searchParameters.CreatedToDate.HasValue)
        {
            query = query.Where(ar => ar.UtcCreatedAt <= searchParameters.CreatedToDate.Value);
        }

        // Order by creation date descending
        query = query.OrderByDescending(ar => ar.UtcCreatedAt);

        // Apply pagination if specified
        if (searchParameters.PageNumber.HasValue && searchParameters.PageSize.HasValue)
        {
            var skip = (searchParameters.PageNumber.Value - 1) * searchParameters.PageSize.Value;
            query = query.Skip(skip).Take(searchParameters.PageSize.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }
}
