# Entity Framework Core Migrations Guide

This document provides a comprehensive guide for working with Entity Framework Core migrations in the Axs project, including initialization, execution, and data model changes.

## Overview

The Axs project supports both SQL Server and SQLite databases through Entity Framework Core. The database provider is controlled by the `DatabaseProvider` setting in appsettings files.

## Project Structure

- **Infrastructure Project**: Contains the DbContext and migration files
- **API Project**: Startup project for EF Core commands
- **Database Provider**: Configurable via `DatabaseProvider` setting (`SqlServer` or `Sqlite`)

## Prerequisites

Ensure you have the Entity Framework Core tools installed:

```bash
dotnet tool install --global dotnet-ef
```

Or update if already installed:

```bash
dotnet tool update --global dotnet-ef
```

## Database Provider Configuration

### SQL Server Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=axs;User Id=sa;Password=YourPassword;TrustServerCertificate=true;MultipleActiveResultSets=true;"
  },
  "DatabaseProvider": "SqlServer"
}
```

### SQLite Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=axs.db"
  },
  "DatabaseProvider": "Sqlite"
}
```

## Common EF Core Commands

All commands should be run from the Service project root directory (`/home/jim/Projects/Axs/Service`).

### 1. Initial Migration Setup

When setting up EF Core migrations for the first time:

```bash
# Create the initial migration
dotnet ef migrations add InitialCreate --project src/Infrastructure --startup-project src/Api

# Apply the migration to create the database
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```

### 2. Creating New Migrations

When you make changes to your data model (entities, DbContext configuration):

```bash
# Create a new migration with a descriptive name
dotnet ef migrations add <MigrationName> --project src/Infrastructure --startup-project src/Api

# Examples:
dotnet ef migrations add AddUserEmailField --project src/Infrastructure --startup-project src/Api
dotnet ef migrations add UpdateAccessRequestTable --project src/Infrastructure --startup-project src/Api
dotnet ef migrations add AddIndexesToPerformanceTables --project src/Infrastructure --startup-project src/Api
```

### 3. Applying Migrations

```bash
# Apply all pending migrations
dotnet ef database update --project src/Infrastructure --startup-project src/Api

# Apply migrations up to a specific migration
dotnet ef database update <MigrationName> --project src/Infrastructure --startup-project src/Api

# Example:
dotnet ef database update AddUserEmailField --project src/Infrastructure --startup-project src/Api
```

### 4. Migration Information Commands

```bash
# List all migrations and their status
dotnet ef migrations list --project src/Infrastructure --startup-project src/Api

# View the SQL that would be executed for pending migrations
dotnet ef migrations script --project src/Infrastructure --startup-project src/Api

# Generate SQL script from one migration to another
dotnet ef migrations script <FromMigration> <ToMigration> --project src/Infrastructure --startup-project src/Api
```

### 5. Database Information Commands

```bash
# Get information about the current database and applied migrations
dotnet ef database info --project src/Infrastructure --startup-project src/Api
```

### 6. Removing Migrations

```bash
# Remove the last migration (only if not applied to database)
dotnet ef migrations remove --project src/Infrastructure --startup-project src/Api

# Force remove (use with caution)
dotnet ef migrations remove --project src/Infrastructure --startup-project src/Api --force
```

### 7. Database Reset and Recreation

```bash
# Drop the database (use with extreme caution)
dotnet ef database drop --project src/Infrastructure --startup-project src/Api

# Recreate from scratch
dotnet ef database drop --project src/Infrastructure --startup-project src/Api
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```

## Environment-Specific Workflows

### Development Environment (SQLite)

1. Update `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=axs.db"
  },
  "DatabaseProvider": "Sqlite"
}
```

2. Run migrations:
```bash
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```

### Production Environment (SQL Server)

1. Ensure `appsettings.json` or production config has:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=axs;User Id=sa;Password=ProdPassword;TrustServerCertificate=true;"
  },
  "DatabaseProvider": "SqlServer"
}
```

2. Generate deployment script:
```bash
dotnet ef migrations script --project src/Infrastructure --startup-project src/Api --output migration-script.sql
```

3. Review and execute the script on production database.

## Best Practices

### Migration Naming
- Use descriptive names: `AddUserEmailField`, `UpdateIndexes`, `RefactorAuthorizationTable`
- Include ticket/issue numbers: `TICKET123_AddUserPreferences`
- Use consistent naming conventions

### Before Creating Migrations
1. **Review Changes**: Ensure all model changes are intentional
2. **Test Locally**: Verify changes work with both SQLite and SQL Server
3. **Backup**: Always backup production databases before applying migrations

### Migration Content Review
1. **Check Generated SQL**: Use `dotnet ef migrations script` to review
2. **Data Loss Prevention**: Ensure migrations don't cause unintended data loss
3. **Performance Impact**: Consider the impact of index changes on large tables

### Testing Migrations
1. **Test Both Providers**: Verify migrations work with both SQLite and SQL Server
2. **Up and Down**: Test both applying and rolling back migrations
3. **Data Integrity**: Verify data integrity after migration

## Troubleshooting

### Common Issues

1. **Migration Already Applied**
   ```
   Error: The migration '...' has already been applied to the database.
   ```
   Solution: Use `dotnet ef migrations list` to check status

2. **Pending Model Changes**
   ```
   Error: Unable to create a migration because pending model changes were detected.
   ```
   Solution: Create a new migration for the changes

3. **Database Provider Mismatch**
   ```
   Error: The database provider in use does not support this operation.
   ```
   Solution: Check `DatabaseProvider` setting in appsettings

4. **Connection String Issues**
   ```
   Error: Unable to connect to database.
   ```
   Solution: Verify connection string and database server availability

### Recovery Steps

1. **Reset Development Database**:
   ```bash
   rm src/Api/axs.db  # For SQLite
   dotnet ef database update --project src/Infrastructure --startup-project src/Api
   ```

2. **Rollback Migration**:
   ```bash
   dotnet ef database update <PreviousMigrationName> --project src/Infrastructure --startup-project src/Api
   ```

3. **Clean Migration State**:
   ```bash
   dotnet ef migrations remove --project src/Infrastructure --startup-project src/Api
   # Recreate the migration with fixes
   dotnet ef migrations add <FixedMigrationName> --project src/Infrastructure --startup-project src/Api
   ```

## File Locations

- **Migrations**: `src/Infrastructure/Migrations/`
- **DbContext**: `src/Infrastructure/Data/AccessDbContext.cs`
- **Configuration**: `src/Infrastructure/Extensions/ServiceCollectionExtensions.cs`
- **SQLite Database**: `src/Api/axs.db` (development)

## Related Documentation

- [Microsoft EF Core Migrations Documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [EF Core Database Providers](https://docs.microsoft.com/en-us/ef/core/providers/)
- [EF Core CLI Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

---

*Last Updated: August 29, 2025*
*Project: Axs Control Service*
