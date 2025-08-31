# User Commands and Queries Implementation

## Overview
This document describes the implementation of User-related CQRS commands and queries using the record pattern, based on the User entity structure.

## Commands Implemented

### 1. **CreateUserCommand**
**File**: `src/Application/Users/Commands/CreateUserCommand.cs`

```csharp
public record CreateUserCommand(
    string GraphId,
    string UserId,
    string DisplayName,
    string PrincipalName,
    bool IsEnabled = true
);
```

**Purpose**: Creates a new user in the system
**Parameters**:
- `GraphId`: Microsoft Graph unique identifier (required)
- `UserId`: Business/external system user identifier (required)
- `DisplayName`: Human-readable user name (required)
- `PrincipalName`: User Principal Name from Active Directory (required)
- `IsEnabled`: User account status flag (default: true)

**Auto-Generated Fields**:
- `Id`: Primary key (auto-generated)
- `LastUpdated`: Set to current timestamp
- `UtcCreatedAt`: Set to current timestamp
- `CreatedByNum`: Set by command handler

### 2. **UpdateUserCommand**
**File**: `src/Application/Users/Commands/UpdateUserCommand.cs`

```csharp
public record UpdateUserCommand(
    int Id,
    string GraphId,
    string UserId,
    string DisplayName,
    string PrincipalName,
    bool IsEnabled = true
);
```

**Purpose**: Updates an existing user
**Parameters**:
- `Id`: Primary key of user to update (required)
- All other properties same as CreateUserCommand

**Auto-Generated Fields**:
- `UtcUpdatedAt`: Set to current timestamp
- `UpdatedByNum`: Set by command handler

### 3. **DeleteUserCommand**
**File**: `src/Application/Users/Commands/DeleteUserCommand.cs`

```csharp
public record DeleteUserCommand(int Id);
```

**Purpose**: Deletes a user by primary key
**Parameters**:
- `Id`: Primary key of user to delete

## Queries Implemented

### 1. **GetUserByIdQuery**
**File**: `src/Application/Users/Queries/GetUserByUserId.cs`

```csharp
public record GetUserByIdQuery(int Id);
```

**Purpose**: Retrieves a user by primary key
**Parameters**:
- `Id`: Primary key of user to retrieve

### 2. **GetUserByUserIdQuery**
**File**: `src/Application/Users/Queries/GetUserByUserId.cs`

```csharp
public record GetUserByUserIdQuery(string UserId);
```

**Purpose**: Retrieves a user by business UserId
**Parameters**:
- `UserId`: Business/external system user identifier

### 3. **GetUsersQuery**
**File**: `src/Application/Users/Queries/GetUsersQuery.cs`

```csharp
public record GetUsersQuery();
```

**Purpose**: Retrieves all users
**Parameters**: None

## Design Patterns Applied

### **Record Pattern**
All commands and queries use C# records for:
- ✅ **Immutability**: Commands are immutable once created
- ✅ **Value Semantics**: Structural equality comparison
- ✅ **Concise Syntax**: Clean, readable declarations
- ✅ **Pattern Matching**: Easy to use in switch expressions

### **CQRS Pattern**
Clear separation between:
- ✅ **Commands**: Modify state (Create, Update, Delete)
- ✅ **Queries**: Read state (Get operations)

### **Consistency with Project Standards**
Following established patterns from Activity entity:
- ✅ **Naming Conventions**: Consistent with ActivityCommand patterns
- ✅ **Parameter Structure**: Similar parameter organization
- ✅ **Default Values**: Sensible defaults for optional parameters

## Usage Examples

### **Creating a User**
```csharp
var command = new CreateUserCommand(
    GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    UserId: "john.doe",
    DisplayName: "John Doe",
    PrincipalName: "john.doe@company.com",
    IsEnabled: true
);

var result = await commandDispatcher.Dispatch<CreateUserCommand, Result<User>>(command);
```

### **Updating a User**
```csharp
var command = new UpdateUserCommand(
    Id: 1,
    GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    UserId: "john.doe",
    DisplayName: "John A. Doe",
    PrincipalName: "john.doe@company.com",
    IsEnabled: true
);

var result = await commandDispatcher.Dispatch<UpdateUserCommand, Result<User>>(command);
```

### **Querying Users**
```csharp
// Get by primary key
var query1 = new GetUserByIdQuery(1);
var user = await queryDispatcher.Dispatch<GetUserByIdQuery, Result<User>>(query1);

// Get by business UserId
var query2 = new GetUserByUserIdQuery("john.doe");
var user = await queryDispatcher.Dispatch<GetUserByUserIdQuery, Result<User>>(query2);

// Get all users
var query3 = new GetUsersQuery();
var users = await queryDispatcher.Dispatch<GetUsersQuery, Result<IEnumerable<User>>>(query3);
```

## Next Steps for Implementation

To complete the User CQRS implementation, these components still need to be created:

### **Command Handlers** (Required)
- `CreateUserCommandHandler`
- `UpdateUserCommandHandler`
- `DeleteUserCommandHandler`

### **Query Handlers** (Required)
- `GetUserByIdQueryHandler`
- `GetUserByUserIdQueryHandler`
- `GetUsersQueryHandler`

### **Validators** (Recommended)
- `CreateUserCommandValidator`
- `UpdateUserCommandValidator`

### **Repository Extensions** (Optional)
- Custom methods in UserRepository for business logic queries

### **API Endpoints** (For Web API)
- User endpoint controllers using these commands/queries

## Validation Considerations

### **Required Fields**
- `GraphId`: Must be valid GUID format
- `UserId`: Must be unique, alphanumeric with periods/underscores
- `DisplayName`: Must not be empty, reasonable length
- `PrincipalName`: Must be valid email/UPN format

### **Business Rules**
- `UserId` must be unique across system
- `GraphId` must be unique across system
- `PrincipalName` must be unique across system
- `IsEnabled` defaults to true for new users

## Integration Points

### **External Systems**
- **Microsoft Graph**: Use `GraphId` for Graph API calls
- **Active Directory**: Use `PrincipalName` for AD operations
- **Business Systems**: Use `UserId` for cross-system references

### **Internal Relationships**
- **Authorizations**: Link via `UserId` foreign key
- **UserGroupMembers**: Link via `UserId` foreign key

---
*Date: August 30, 2025*
*Implementation: User Commands and Queries*
