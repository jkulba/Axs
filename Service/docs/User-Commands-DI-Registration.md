# User Commands Registration in ServiceCollectionExtensions

## Overview
This document describes the registration of User commands, queries, and their handlers in the Dependency Injection container through ServiceCollectionExtensions.

## Changes Made

### 1. **Application ServiceCollectionExtensions Updates**
**File**: `src/Application/ServiceCollectionExtensions.cs`

#### **Added Using Statements**
```csharp
using Application.Users.Commands;
using Application.Users.Queries;
```

#### **Registered Query Handlers**
```csharp
services.AddScoped<IQueryHandler<GetUserByIdQuery, Result<User>>, GetUserByIdQueryHandler>();
services.AddScoped<IQueryHandler<GetUserByUserIdQuery, Result<User>>, GetUserByUserIdQueryHandler>();
services.AddScoped<IQueryHandler<GetUsersQuery, Result<IEnumerable<User>>>, GetUsersQueryHandler>();
```

#### **Registered Command Handlers**
```csharp
services.AddScoped<ICommandHandler<CreateUserCommand, Result<User>>, CreateUserCommandHandler>();
services.AddScoped<ICommandHandler<UpdateUserCommand, Result<User>>, UpdateUserCommandHandler>();
services.AddScoped<ICommandHandler<DeleteUserCommand, Result<int>>, DeleteUserCommandHandler>();
```

### 2. **Infrastructure ServiceCollectionExtensions Updates**
**File**: `src/Infrastructure/Extensions/ServiceCollectionExtensions.cs`

#### **Registered Repository**
```csharp
services.AddScoped<IUserRepository, UserRepository>();
```

### 3. **Handler Interface Implementation Fixes**

All User command and query handlers were updated to implement the proper interfaces:

#### **Query Handlers Fixed**
- `GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, Result<User>>`
- `GetUserByUserIdQueryHandler : IQueryHandler<GetUserByUserIdQuery, Result<User>>`
- `GetUsersQueryHandler : IQueryHandler<GetUsersQuery, Result<IEnumerable<User>>>`

#### **Command Handlers Fixed**
- `CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<User>>`
- `UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Result<User>>`
- `DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Result<int>>`

## Registration Pattern

### **Query Handler Registration Pattern**
```csharp
services.AddScoped<IQueryHandler<TQuery, TResult>, TQueryHandler>();
```

**Example**:
```csharp
services.AddScoped<IQueryHandler<GetUserByIdQuery, Result<User>>, GetUserByIdQueryHandler>();
```

### **Command Handler Registration Pattern**
```csharp
services.AddScoped<ICommandHandler<TCommand, TResult>, TCommandHandler>();
```

**Example**:
```csharp
services.AddScoped<ICommandHandler<CreateUserCommand, Result<User>>, CreateUserCommandHandler>();
```

## Complete User CQRS Registration

### **Commands Registered**
1. **CreateUserCommand** → CreateUserCommandHandler
   - Input: User creation data
   - Output: `Result<User>`

2. **UpdateUserCommand** → UpdateUserCommandHandler
   - Input: User ID + update data
   - Output: `Result<User>`

3. **DeleteUserCommand** → DeleteUserCommandHandler
   - Input: User ID
   - Output: `Result<int>` (number of deleted records)

### **Queries Registered**
1. **GetUserByIdQuery** → GetUserByIdQueryHandler
   - Input: Primary key ID
   - Output: `Result<User>`

2. **GetUserByUserIdQuery** → GetUserByUserIdQueryHandler
   - Input: Business UserId string
   - Output: `Result<User>`

3. **GetUsersQuery** → GetUsersQueryHandler
   - Input: None
   - Output: `Result<IEnumerable<User>>`

## Dependency Injection Flow

### **Registration Hierarchy**
```
Application ServiceCollectionExtensions
├── Command Handlers (ICommandHandler<,>)
├── Query Handlers (IQueryHandler<,>)
└── Validation Behaviors

Infrastructure ServiceCollectionExtensions
├── DbContext (AccessDbContext)
├── Generic Repository (IGenericRepository<>)
└── Specific Repositories (IUserRepository)
```

### **Runtime Resolution**
When a User command/query is dispatched:

1. **Command/Query Dispatcher** resolves the appropriate handler
2. **Handler** receives injected `IUserRepository`
3. **Repository** receives injected `AccessDbContext`
4. **DbContext** connects to configured database (SQLite/SQL Server)

## Usage Examples

### **Command Dispatching**
```csharp
// Inject the command dispatcher
ICommandDispatcher _commandDispatcher;

// Create user
var createCommand = new CreateUserCommand("graphId", "userId", "Display Name", "user@domain.com");
var createResult = await _commandDispatcher.Dispatch<CreateUserCommand, Result<User>>(createCommand);

// Update user
var updateCommand = new UpdateUserCommand(1, "graphId", "userId", "New Name", "user@domain.com");
var updateResult = await _commandDispatcher.Dispatch<UpdateUserCommand, Result<User>>(updateCommand);

// Delete user
var deleteCommand = new DeleteUserCommand(1);
var deleteResult = await _commandDispatcher.Dispatch<DeleteUserCommand, Result<int>>(deleteCommand);
```

### **Query Dispatching**
```csharp
// Inject the query dispatcher
IQueryDispatcher _queryDispatcher;

// Get by ID
var query1 = new GetUserByIdQuery(1);
var user = await _queryDispatcher.Dispatch<GetUserByIdQuery, Result<User>>(query1);

// Get by UserId
var query2 = new GetUserByUserIdQuery("john.doe");
var user = await _queryDispatcher.Dispatch<GetUserByUserIdQuery, Result<User>>(query2);

// Get all users
var query3 = new GetUsersQuery();
var users = await _queryDispatcher.Dispatch<GetUsersQuery, Result<IEnumerable<User>>>(query3);
```

## Benefits of This Registration

### **1. Dependency Injection**
- ✅ Automatic resolution of dependencies
- ✅ Scoped lifetime management
- ✅ Easy testing with mock implementations

### **2. CQRS Pattern Compliance**
- ✅ Clear separation of commands and queries
- ✅ Single responsibility per handler
- ✅ Consistent interface contracts

### **3. Validation & Behaviors**
- ✅ Automatic validation through FluentValidation
- ✅ Performance monitoring through behaviors
- ✅ Cross-cutting concerns handled transparently

### **4. Type Safety**
- ✅ Compile-time verification of handler contracts
- ✅ Strong typing throughout the pipeline
- ✅ IntelliSense support in IDEs

## Next Steps

### **For API Integration**
Create User endpoints that use these registered commands/queries:
```csharp
app.MapPost("/api/users", async (CreateUserCommand command, ICommandDispatcher dispatcher) => 
{
    var result = await dispatcher.Dispatch<CreateUserCommand, Result<User>>(command);
    return result.IsSuccess ? Results.Created($"/api/users/{result.Value.Id}", result.Value) 
                            : result.ToProblemDetails();
});
```

### **For Validation**
Add FluentValidation validators:
- `CreateUserCommandValidator`
- `UpdateUserCommandValidator`

The validators will be automatically discovered and registered by the existing FluentValidation assembly scanning.

---
*Date: August 30, 2025*
*Implementation: User Commands Registration in DI Container*
