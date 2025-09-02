# AXS Database Query Guide

## Database Overview

The `axs.db` is a SQLite database that stores access control and authorization data for the AXS (Access Control System). The database contains information about users, activities, access requests, and authorization relationships.

## Database Schema

### Core Tables

#### **Users** Table

Stores user information from the identity system.

```sql
-- Key columns: Id, GraphId, UserId, DisplayName, PrincipalName, IsEnabled
SELECT * FROM Users LIMIT 5;
```

#### **Activities** Table

Stores activity definitions that users can be authorized for.

```sql
-- Key columns: Id, ActivityCode, ActivityName, Description, IsActive
SELECT * FROM Activities LIMIT 5;
```

#### **AccessRequest** Table

Stores access requests submitted by users.

```sql
-- Key columns: Id, RequestCode, UserName, JobNumber, CycleNumber, ActivityCode
SELECT * FROM AccessRequest LIMIT 5;
```

#### **Authorizations** Table

Stores individual user authorizations for specific activities and job numbers.

```sql
-- Key columns: AuthorizationId, JobNumber, UserId, ActivityId, IsAuthorized
SELECT * FROM Authorizations LIMIT 5;
```

#### **UserGroups** Table

Stores user group definitions.

```sql
-- Key columns: GroupId, GroupName, Description, GroupOwner
SELECT * FROM UserGroups LIMIT 5;
```

#### **UserGroupMembers** Table

Maps users to groups (many-to-many relationship).

```sql
-- Key columns: GroupId, UserId
SELECT * FROM UserGroupMembers LIMIT 5;
```

#### **GroupAuthorizations** Table

Stores group-level authorizations for activities and job numbers.

```sql
-- Key columns: AuthorizationId, JobNumber, GroupId, ActivityId, IsAuthorized
SELECT * FROM GroupAuthorizations LIMIT 5;
```

## Common Query Examples

### 1. View All Tables and Row Counts

```sql
SELECT 'Users' as TableName, COUNT(*) as RowCount FROM Users
UNION ALL
SELECT 'Activities', COUNT(*) FROM Activities
UNION ALL
SELECT 'AccessRequest', COUNT(*) FROM AccessRequest
UNION ALL
SELECT 'Authorizations', COUNT(*) FROM Authorizations
UNION ALL
SELECT 'UserGroups', COUNT(*) FROM UserGroups
UNION ALL
SELECT 'UserGroupMembers', COUNT(*) FROM UserGroupMembers
UNION ALL
SELECT 'GroupAuthorizations', COUNT(*) FROM GroupAuthorizations;
```

### 2. Find User Authorizations

```sql
-- Get all authorizations for a specific user
SELECT
    u.DisplayName,
    u.UserId,
    a.ActivityCode,
    a.ActivityName,
    auth.JobNumber,
    auth.IsAuthorized,
    auth.UtcCreatedAt
FROM Authorizations auth
JOIN Users u ON auth.UserId = u.UserId
JOIN Activities a ON auth.ActivityId = a.Id
WHERE u.UserId = 'your_user_id_here'
ORDER BY auth.UtcCreatedAt DESC;
```

### 3. Find Group Authorizations

```sql
-- Get all group authorizations
SELECT
    ug.GroupName,
    a.ActivityCode,
    a.ActivityName,
    ga.JobNumber,
    ga.IsAuthorized
FROM GroupAuthorizations ga
JOIN UserGroups ug ON ga.GroupId = ug.GroupId
JOIN Activities a ON ga.ActivityId = a.Id
WHERE ga.IsAuthorized = 1
ORDER BY ug.GroupName, a.ActivityCode;
```

### 4. Recent Access Requests

```sql
-- View recent access requests
SELECT
    UserName,
    JobNumber,
    CycleNumber,
    ActivityCode,
    ApplicationName,
    Workstation,
    UtcCreatedAt
FROM AccessRequest
ORDER BY UtcCreatedAt DESC
LIMIT 20;
```

### 5. User Group Memberships

```sql
-- See which users belong to which groups
SELECT
    ug.GroupName,
    u.DisplayName,
    u.UserId,
    u.PrincipalName
FROM UserGroupMembers ugm
JOIN UserGroups ug ON ugm.GroupId = ug.GroupId
LEFT JOIN Users u ON ugm.UserId = u.UserId
ORDER BY ug.GroupName, u.DisplayName;
```

### 6. Activity Usage Statistics

```sql
-- Count authorizations per activity
SELECT
    a.ActivityCode,
    a.ActivityName,
    COUNT(auth.AuthorizationId) as DirectAuthorizations,
    COUNT(ga.AuthorizationId) as GroupAuthorizations
FROM Activities a
LEFT JOIN Authorizations auth ON a.Id = auth.ActivityId AND auth.IsAuthorized = 1
LEFT JOIN GroupAuthorizations ga ON a.Id = ga.ActivityId AND ga.IsAuthorized = 1
GROUP BY a.Id, a.ActivityCode, a.ActivityName
ORDER BY DirectAuthorizations DESC, GroupAuthorizations DESC;
```

### 7. Users Without Direct Authorizations

```sql
-- Find users who only have group-based access
SELECT DISTINCT
    u.DisplayName,
    u.UserId,
    u.PrincipalName
FROM Users u
LEFT JOIN Authorizations auth ON u.UserId = auth.UserId
WHERE auth.UserId IS NULL
AND u.IsEnabled = 1
ORDER BY u.DisplayName;
```

### 8. Find Effective User Permissions

```sql
-- Get all effective permissions for a user (direct + group-based)
WITH UserDirectAuth AS (
    SELECT
        u.UserId,
        u.DisplayName,
        a.ActivityCode,
        a.ActivityName,
        auth.JobNumber,
        'Direct' as AuthType
    FROM Users u
    JOIN Authorizations auth ON u.UserId = auth.UserId
    JOIN Activities a ON auth.ActivityId = a.Id
    WHERE auth.IsAuthorized = 1
),
UserGroupAuth AS (
    SELECT
        u.UserId,
        u.DisplayName,
        a.ActivityCode,
        a.ActivityName,
        ga.JobNumber,
        'Group (' || ug.GroupName || ')' as AuthType
    FROM Users u
    JOIN UserGroupMembers ugm ON u.UserId = ugm.UserId
    JOIN GroupAuthorizations ga ON ugm.GroupId = ga.GroupId
    JOIN Activities a ON ga.ActivityId = a.Id
    JOIN UserGroups ug ON ga.GroupId = ug.GroupId
    WHERE ga.IsAuthorized = 1
)
SELECT * FROM UserDirectAuth
UNION ALL
SELECT * FROM UserGroupAuth
WHERE UserId = 'your_user_id_here'
ORDER BY JobNumber, ActivityCode, AuthType;
```

### 9. Audit Trail Queries

```sql
-- Recent authorization changes
SELECT
    'Direct Auth' as Type,
    u.DisplayName,
    a.ActivityCode,
    auth.JobNumber,
    auth.IsAuthorized,
    auth.UtcCreatedAt,
    auth.CreatedByNum
FROM Authorizations auth
JOIN Users u ON auth.UserId = u.UserId
JOIN Activities a ON auth.ActivityId = a.Id
WHERE auth.UtcCreatedAt >= date('now', '-7 days')

UNION ALL

SELECT
    'Group Auth' as Type,
    ug.GroupName,
    a.ActivityCode,
    ga.JobNumber,
    ga.IsAuthorized,
    ga.UtcCreatedAt,
    ga.CreatedByNum
FROM GroupAuthorizations ga
JOIN UserGroups ug ON ga.GroupId = ug.GroupId
JOIN Activities a ON ga.ActivityId = a.Id
WHERE ga.UtcCreatedAt >= date('now', '-7 days')

ORDER BY UtcCreatedAt DESC;
```

### 10. Access Request Analysis

```sql
-- Analyze access request patterns
SELECT
    ActivityCode,
    COUNT(*) as RequestCount,
    COUNT(DISTINCT UserName) as UniqueUsers,
    COUNT(DISTINCT JobNumber) as UniqueJobs,
    MIN(UtcCreatedAt) as FirstRequest,
    MAX(UtcCreatedAt) as LastRequest
FROM AccessRequest
WHERE UtcCreatedAt >= date('now', '-30 days')
GROUP BY ActivityCode
ORDER BY RequestCount DESC;
```

### 11. Orphaned Records Detection

```sql
-- Find authorization records with missing references
SELECT 'Orphaned Authorizations - Missing Users' as Issue, COUNT(*) as Count
FROM Authorizations auth
LEFT JOIN Users u ON auth.UserId = u.UserId
WHERE u.UserId IS NULL

UNION ALL

SELECT 'Orphaned Authorizations - Missing Activities' as Issue, COUNT(*) as Count
FROM Authorizations auth
LEFT JOIN Activities a ON auth.ActivityId = a.Id
WHERE a.Id IS NULL

UNION ALL

SELECT 'Orphaned Group Members - Missing Users' as Issue, COUNT(*) as Count
FROM UserGroupMembers ugm
LEFT JOIN Users u ON ugm.UserId = u.UserId
WHERE u.UserId IS NULL

UNION ALL

SELECT 'Orphaned Group Members - Missing Groups' as Issue, COUNT(*) as Count
FROM UserGroupMembers ugm
LEFT JOIN UserGroups ug ON ugm.GroupId = ug.GroupId
WHERE ug.GroupId IS NULL;
```

### 12. Job Number Analysis

```sql
-- Analyze job number patterns and authorization coverage
SELECT
    JobNumber,
    COUNT(DISTINCT auth.UserId) as DirectUserAuths,
    COUNT(DISTINCT ga.GroupId) as GroupAuths,
    COUNT(DISTINCT a.ActivityCode) as ActivitiesInvolved
FROM (
    SELECT DISTINCT JobNumber FROM Authorizations
    UNION
    SELECT DISTINCT JobNumber FROM GroupAuthorizations
    UNION
    SELECT DISTINCT JobNumber FROM AccessRequest
) jobs
LEFT JOIN Authorizations auth ON jobs.JobNumber = auth.JobNumber AND auth.IsAuthorized = 1
LEFT JOIN GroupAuthorizations ga ON jobs.JobNumber = ga.JobNumber AND ga.IsAuthorized = 1
LEFT JOIN Activities a ON (auth.ActivityId = a.Id OR ga.ActivityId = a.Id)
GROUP BY JobNumber
ORDER BY DirectUserAuths DESC, GroupAuths DESC;
```

## How to Run Queries

### Option 1: Using sqlite3 CLI

```bash
# Navigate to the API directory
cd /path/to/Service/src/Api

# Open the database
sqlite3 axs.db

# Run your queries
.tables
SELECT * FROM Users LIMIT 5;
.quit
```

### Option 2: Using DB Browser for SQLite

1. Install DB Browser for SQLite
2. Open the `axs.db` file located in `/src/Api/axs.db`
3. Use the "Execute SQL" tab to run queries

### Option 3: Using VS Code Extensions

- Install "SQLite Viewer" or "SQLite" extension
- Open the `axs.db` file in VS Code
- Run queries directly in the editor

### Option 4: Using .NET CLI with Entity Framework

```bash
# From the project root
cd /home/jim/Projects/Axs/Service

# Generate SQL script to see current schema
dotnet ef migrations script --project src/Infrastructure --startup-project src/Api
```

## Tips for Querying

1. **Always use LIMIT** when exploring data to avoid overwhelming output
2. **Check table relationships** - Use JOINs to get meaningful data
3. **Filter by dates** using `UtcCreatedAt` and `UtcUpdatedAt` columns
4. **Use WHERE clauses** to filter by `IsEnabled`, `IsActive`, `IsAuthorized` flags
5. **Order results** by relevant timestamp columns for chronological views
6. **Use CTEs (Common Table Expressions)** for complex queries involving multiple data sources
7. **Consider performance** - Add WHERE clauses to limit result sets, especially on large tables

## Database Schema Relationships

```
Users (1) ←→ (M) Authorizations (M) ←→ (1) Activities
Users (1) ←→ (M) UserGroupMembers (M) ←→ (1) UserGroups
UserGroups (1) ←→ (M) GroupAuthorizations (M) ←→ (1) Activities
```

## Common Use Cases

### Security Auditing

- Use queries 8, 9, and 11 to audit user permissions and find security issues
- Query 7 helps identify users who may need direct authorizations

### Access Management

- Query 2 and 8 help administrators understand what access a user has
- Query 5 shows group membership structure
- Query 3 shows group-level permissions

### Reporting and Analytics

- Query 6 and 10 provide usage statistics
- Query 12 analyzes job number patterns
- Query 1 gives overall system health metrics

### Troubleshooting

- Query 11 helps find data integrity issues
- Queries 2-5 help diagnose user access problems
- Query 4 helps track recent access request activity

## Database Location

```
/home/jim/Projects/Axs/Service/src/Api/axs.db
```

---

_Last updated: September 1, 2025_

