# User Entity Implementation

## Overview
This document describes the formatting and completion of the User entity class in the Axs project domain layer.

## Changes Made

### 1. **Formatted and Completed User Class**
**File**: `src/Domain/Entities/User.cs`

**Before** (Incomplete):
```csharp
namespace Domain.Entities;

public class User
{
string GraphId
string UserId
string DisplayName
string PrincipalName
bool IsEnabled
DateTime LastUpdated    
}
```

**After** (Complete):
```csharp
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string GraphId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string PrincipalName { get; set; } = null!;
    public bool IsEnabled { get; set; } = true;
    public DateTime LastUpdated { get; set; }
    public DateTime UtcCreatedAt { get; set; }
    public string? CreatedByNum { get; set; }
    public DateTime? UtcUpdatedAt { get; set; }
    public string? UpdatedByNum { get; set; }

    // Navigation properties
    public virtual ICollection<Authorization> Authorizations { get; set; } = new List<Authorization>();
    public virtual ICollection<UserGroupMember> UserGroupMemberships { get; set; } = new List<UserGroupMember>();
}
```

### 2. **Key Improvements Made**

#### **Formatting & Structure**
- ✅ Added proper `using` statements
- ✅ Added proper access modifiers (`public`)
- ✅ Added property getters/setters (`{ get; set; }`)
- ✅ Added proper null-reference annotations (`= null!`, `?`)
- ✅ Added consistent indentation and spacing

#### **Standard Entity Properties**
- ✅ **Primary Key**: `Id` (follows standard convention for Generic Repository)
- ✅ **Audit Fields**: `UtcCreatedAt`, `CreatedByNum`, `UtcUpdatedAt`, `UpdatedByNum`
- ✅ **Default Values**: `IsEnabled = true`

#### **Navigation Properties**
- ✅ **Authorizations**: One-to-many relationship with Authorization entity
- ✅ **UserGroupMemberships**: One-to-many relationship with UserGroupMember entity
- ✅ **Virtual Properties**: Enables lazy loading

### 3. **Database Configuration**
**File**: `src/Infrastructure/Data/AccessDbContext.cs`

#### **DbSet Addition**
```csharp
public DbSet<User> Users { get; set; }
```

#### **Entity Configuration**
```csharp
// Configure User
modelBuilder.Entity<User>(entity =>
{
    entity.HasKey(e => e.Id);
    entity.Property(e => e.GraphId).IsRequired().HasMaxLength(255);
    entity.Property(e => e.UserId).IsRequired().HasMaxLength(255);
    entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(255);
    entity.Property(e => e.PrincipalName).IsRequired().HasMaxLength(255);
    entity.Property(e => e.CreatedByNum).HasMaxLength(50);
    entity.Property(e => e.UpdatedByNum).HasMaxLength(50);

    // Create unique indexes
    entity.HasIndex(e => e.GraphId).IsUnique();
    entity.HasIndex(e => e.UserId).IsUnique();
    entity.HasIndex(e => e.PrincipalName).IsUnique();
});
```

### 4. **Relationship Configurations**

#### **Authorization Entity**
- ✅ Added optional `User` navigation property
- ✅ Configured foreign key relationship using `UserId`
- ✅ Set delete behavior to `SetNull` (for external user systems)

#### **UserGroupMember Entity**
- ✅ Added optional `User` navigation property  
- ✅ Configured foreign key relationship using `UserId`
- ✅ Set delete behavior to `SetNull` (for external user systems)

## Entity Properties Explained

### **Identity Properties**
- **`Id`**: Primary key for internal database operations
- **`GraphId`**: Microsoft Graph unique identifier
- **`UserId`**: Business/external system user identifier
- **`PrincipalName`**: User Principal Name (UPN) from Active Directory

### **Display Properties**
- **`DisplayName`**: Human-readable user name
- **`IsEnabled`**: User account status flag
- **`LastUpdated`**: Last synchronization timestamp

### **Audit Properties**
- **`UtcCreatedAt`**: Record creation timestamp
- **`CreatedByNum`**: User/system that created the record
- **`UtcUpdatedAt`**: Last modification timestamp
- **`UpdatedByNum`**: User/system that last modified the record

## Database Migration

### **Migration Created**
- ✅ **Migration**: `AddUserEntity`
- ✅ **Status**: Ready to apply
- ✅ **Tables**: Will create `Users` table with proper indexes

### **Apply Migration**
```bash
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```

## Generic Repository Benefits

The User entity now fully supports the Generic Repository pattern:

```csharp
// ✅ All CRUD operations work:
await _userRepository.GetByIdAsync(1);        // Get by primary key
await _userRepository.GetAllAsync();          // Get all users
await _userRepository.AddAsync(newUser);      // Create user
await _userRepository.UpdateAsync(user);     // Update user
await _userRepository.DeleteAsync(1);        // Delete user
await _userRepository.ExistsAsync(1);        // Check existence
```

## Usage Patterns

### **External System Integration**
The User entity is designed to work with external user systems (like Active Directory):
- `UserId` can reference external user IDs
- `GraphId` for Microsoft Graph integration
- `PrincipalName` for UPN-based lookups
- Optional relationships allow for missing external users

### **Authorization Integration**
```csharp
// Check user authorizations
var user = await _userRepository.GetByIdAsync(userId);
var userAuthorizations = user.Authorizations;

// Check user group memberships
var userGroups = user.UserGroupMemberships.Select(m => m.Group);
```

## Best Practices Applied

1. **✅ Null-Reference Annotations**: Proper use of `= null!` and `?`
2. **✅ Standard Conventions**: Follows EF Core and C# naming conventions
3. **✅ Consistent Patterns**: Matches other entities in the domain
4. **✅ Defensive Design**: Optional relationships for external dependencies
5. **✅ Performance**: Proper indexing on lookup fields
6. **✅ Maintainability**: Clear separation of concerns and consistent structure

---
*Date: August 30, 2025*
*Change: User Entity Formatting and Completion*
