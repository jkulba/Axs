using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ActivityRepository : GenericRepository<Activity>, IActivityRepository
{
    public ActivityRepository(AccessDbContext context) : base(context)
    {
        // All CRUD operations are inherited from GenericRepository<Activity>
        // Add Activity-specific methods here when needed
    }
}