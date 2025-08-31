# Activity Entity Primary Key Standardization

## Overview
This document describes the changes made to standardize the Activity entity primary key to work with the Generic Repository pattern.

## Problem
The `Activity` entity was using `ActivityId` as its primary key, but the `GenericRepository<T>` was hardcoded to look for a property named `"Id"`. This caused the generic repository methods to fail when used with the Activity entity.

## Solution
**Option Selected**: Update the Activity entity to use `Id` instead of `ActivityId`

This approach was chosen because:
- ✅ Maintains consistency with standard EF Core conventions
- ✅ Allows full use of the Generic Repository pattern
- ✅ Follows the principle of least surprise (Id is the conventional primary key name)
- ✅ Requires fewer changes than making the Generic Repository flexible

## Changes Made

### 1. Domain Entity
**File**: `src/Domain/Entities/Activity.cs`
- Changed primary key from `ActivityId` to `Id`

### 2. Database Configuration
**File**: `src/Infrastructure/Data/AccessDbContext.cs`
- Updated `HasKey(e => e.ActivityId)` to `HasKey(e => e.Id)`

### 3. Application Layer
**Files Updated**:
- `src/Application/Activities/Commands/UpdateActivityCommand.cs` - Parameter renamed from `ActivityId` to `Id`
- `src/Application/Activities/Queries/GetActivityByIdQuery.cs` - Parameter renamed from `ActivityId` to `Id`
- `src/Application/Activities/Commands/UpdateActivityCommandHandler.cs` - Updated to use `command.Id`
- `src/Application/Activities/Queries/GetActivityByIdQueryHandler.cs` - Updated to use `query.Id`
- `src/Application/Activities/Validators/UpdateActivityCommandValidator.cs` - Updated validation rule

### 4. API Layer
**File**: `src/Api/Endpoints/ActivityEndpoints.cs`
- Updated created resource URL to use `result.Value.Id`
- Updated route parameter validation to use `command.Id`

### 5. Database Migration
**Migration**: `20250830022352_RenameActivityIdToId`
- Renames the `ActivityId` column to `Id` in the Activities table
- Applied successfully to SQLite database

## Foreign Key Relationships
**Note**: The `Authorization` and `GroupAuthorization` entities still use `ActivityId` as their foreign key property. This is correct because:
- Foreign key properties can have different names than the referenced primary key
- EF Core correctly maps `ActivityId` foreign keys to the `Id` primary key
- The relationship configuration in `AccessDbContext` handles this mapping

## Generic Repository Benefits
With this change, the Activity entity now fully benefits from the Generic Repository:

```csharp
// ✅ These now work correctly:
await _activityRepository.GetByIdAsync(1);        // Uses Activity.Id
await _activityRepository.GetAllAsync();          // Returns all activities
await _activityRepository.ExistsAsync(1);        // Checks if Activity.Id exists
```

## Testing Verification
1. **Migration Applied**: ✅ Database column renamed from `ActivityId` to `Id`
2. **Compilation**: ✅ No compilation errors
3. **Entity Framework**: ✅ Properly maps relationships

## API Impact
The API endpoints continue to work as expected:
- `GET /api/activities/{id}` - Still works with integer ID
- `POST /api/activities` - Returns created resource with correct ID
- `PUT /api/activities/{id}` - Updates using the ID parameter

## Alternative Approaches Considered

### Option 2: Make Generic Repository Flexible
Could have modified `GenericRepository<T>` to accept a key property name, but this would:
- ❌ Complicate the generic repository pattern
- ❌ Require changes to all entities and repositories
- ❌ Break the convention-based approach

### Option 3: Custom Activity Repository
Could have avoided the generic repository for Activity, but this would:
- ❌ Duplicate common CRUD operations
- ❌ Lose the benefits of the generic pattern
- ❌ Create inconsistency across repositories

## Best Practices Applied
1. **Convention over Configuration**: Used standard `Id` naming convention
2. **Minimal Impact**: Changed only what was necessary
3. **Database Migration**: Proper migration to preserve data
4. **Comprehensive Update**: Updated all related code consistently

---
*Date: August 30, 2025*
*Change: Activity Entity Primary Key Standardization*
