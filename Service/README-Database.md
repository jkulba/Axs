# Access Request System - Database Setup

This document describes the Entity Framework Core setup for the Access Request System with SQL Server.

## Overview

The system includes:
- **AccessDbContext**: Entity Framework DbContext for SQL Server
- **AccessRequestRepository**: Implementation of IAccessRequestRepository
- **Generic Repository Pattern**: Base repository for common CRUD operations
- **Database Migrations**: EF Core migrations for database schema management

## Database Schema

The system manages the following entities:

### AccessRequest
- Primary table for access requests
- Contains request details, user information, and job/activity data
- Indexed on RequestCode, JobNumber, and UserName for performance

### Authorization
- User-specific authorizations for activities
- Links users to specific activities within jobs

### Activity
- Master data for activities that can be authorized
- Contains activity codes, names, and descriptions

### UserGroup & UserGroupMember
- User group management
- Many-to-many relationship between users and groups

### GroupAuthorization
- Group-based authorizations
- Allows authorizing entire groups for specific activities

## Configuration

### Connection String
The system expects a connection string named "DefaultConnection" in appsettings.json:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=axs;User Id=sa;Password=YourPassword;TrustServerCertificate=true;MultipleActiveResultSets=true;"
  }
}
```

### Dependency Injection
The Infrastructure layer is registered in Program.cs:

```csharp
builder.Services.AddInfrastructure(builder.Configuration);
```

This registers:
- AccessDbContext with SQL Server
- IGenericRepository<T> implementation
- IAccessRequestRepository implementation

## Database Migration Commands

### Create a new migration:
```bash
cd src/Infrastructure
dotnet ef migrations add <MigrationName> --startup-project ../Api --context AccessDbContext
```

### Update database:
```bash
cd src/Infrastructure
dotnet ef database update --startup-project ../Api --context AccessDbContext
```

### Remove last migration:
```bash
cd src/Infrastructure
dotnet ef migrations remove --startup-project ../Api --context AccessDbContext
```

## Repository Usage

### IAccessRequestRepository Methods

The repository provides the following methods beyond the generic CRUD operations:

```csharp
// Get by unique request code
Task<AccessRequest?> GetByRequestCodeAsync(Guid requestCode);

// Get all requests for a specific job
Task<IEnumerable<AccessRequest>> GetByJobNumberAsync(int jobNumber);

// Get all requests for a specific user
Task<IEnumerable<AccessRequest>> GetByUserNameAsync(string userName);

// Check if a request code exists
Task<bool> ExistsByRequestCodeAsync(Guid requestCode);
```

### Example Usage in Controllers

```csharp
[ApiController]
[Route("api/[controller]")]
public class AccessRequestsController : ControllerBase
{
    private readonly IAccessRequestRepository _repository;

    public AccessRequestsController(IAccessRequestRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccessRequest>> GetById(int id)
    {
        var request = await _repository.GetByIdAsync(id);
        return request == null ? NotFound() : Ok(request);
    }

    [HttpPost]
    public async Task<ActionResult<AccessRequest>> Create(AccessRequest request)
    {
        request.RequestCode = Guid.NewGuid();
        request.UtcCreatedAt = DateTime.UtcNow;
        
        var created = await _repository.AddAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.RequestId }, created);
    }
}
```

## Database Indexes

The following indexes are automatically created for optimal query performance:

### AccessRequests Table
- Unique index on `RequestCode`
- Index on `JobNumber`
- Index on `UserName`

### Authorization Table
- Unique composite index on `JobNumber`, `UserId`, `ActivityId`

### Activity Table
- Unique index on `ActivityCode`

### UserGroup Table
- Unique index on `GroupName`

### GroupAuthorization Table
- Unique composite index on `JobNumber`, `GroupId`, `ActivityId`

## API Endpoints

The AccessRequestsController provides the following endpoints:

- `GET /api/accessrequests` - Get all access requests
- `GET /api/accessrequests/{id}` - Get by ID
- `GET /api/accessrequests/by-request-code/{requestCode}` - Get by request code
- `GET /api/accessrequests/by-job-number/{jobNumber}` - Get by job number
- `GET /api/accessrequests/by-user/{userName}` - Get by user name
- `POST /api/accessrequests` - Create new request
- `PUT /api/accessrequests/{id}` - Update existing request
- `DELETE /api/accessrequests/{id}` - Delete request
- `GET /api/accessrequests/exists/{requestCode}` - Check if request code exists

## Error Handling

All repository methods include proper error handling and logging. Exceptions are caught at the controller level and appropriate HTTP status codes are returned.

## Next Steps

1. **Run the initial migration** to create the database schema
2. **Test the API endpoints** using Swagger UI or your preferred API testing tool
3. **Add seed data** if needed for initial testing
4. **Implement additional repositories** for other entities as needed
5. **Add business logic services** in the Application layer

## Troubleshooting

### Common Issues

1. **Connection String**: Ensure the SQL Server instance is running and accessible
2. **Migrations**: If migrations fail, check that the startup project is correctly specified
3. **Dependencies**: Ensure all NuGet packages are restored properly

### Logging

The application uses Serilog for logging. Database operations and errors will be logged to both console and file outputs as configured in appsettings.json.
