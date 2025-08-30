# CreateUserCommandValidator Implementation

## Overview
This document describes the implementation of the CreateUserCommandValidator for validating User creation requests in the Axs project.

## Validator Details

### **File Location**
`src/Application/Users/Validators/CreateUserCommandValidator.cs`

### **Validation Rules Applied**

#### **1. GraphId Validation**
```csharp
RuleFor(x => x.GraphId)
    .NotEmpty()
    .MaximumLength(255)
    .Matches(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")
```
- **Required**: Cannot be empty or null
- **Max Length**: 255 characters (database constraint)
- **Format**: Must be a valid GUID format (Microsoft Graph ID)
- **Example**: `a1b2c3d4-e5f6-7890-abcd-ef1234567890`

#### **2. UserId Validation**
```csharp
RuleFor(x => x.UserId)
    .NotEmpty()
    .MaximumLength(255)
    .Matches(@"^[a-zA-Z0-9._-]+$")
```
- **Required**: Cannot be empty or null
- **Max Length**: 255 characters (database constraint)
- **Format**: Alphanumeric with dots, underscores, and hyphens only
- **Examples**: `john.doe`, `user_123`, `test-user`

#### **3. DisplayName Validation**
```csharp
RuleFor(x => x.DisplayName)
    .NotEmpty()
    .MaximumLength(255)
    .Must(name => !string.IsNullOrWhiteSpace(name))
```
- **Required**: Cannot be empty or null
- **Max Length**: 255 characters (database constraint)
- **Content**: Cannot be only whitespace
- **Examples**: `John Doe`, `Jane Smith`

#### **4. PrincipalName Validation**
```csharp
RuleFor(x => x.PrincipalName)
    .NotEmpty()
    .MaximumLength(255)
    .EmailAddress()
```
- **Required**: Cannot be empty or null
- **Max Length**: 255 characters (database constraint)
- **Format**: Must be valid email address format (UPN format)
- **Examples**: `john.doe@company.com`, `user@domain.org`

#### **5. IsEnabled Validation**
```csharp
RuleFor(x => x.IsEnabled)
    .NotNull()
```
- **Required**: Must have a boolean value (not null)
- **Default**: true (set in command definition)

## Registration

### **Automatic Registration**
The validator is automatically registered through FluentValidation's assembly scanning:

```csharp
// In Application/ServiceCollectionExtensions.cs
services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
```

**No manual registration required** - FluentValidation discovers and registers all validators in the assembly.

## Usage in Pipeline

### **Automatic Validation**
The validator runs automatically through the validation pipeline behavior:

```csharp
// Command pipeline includes validation behavior
services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(CommandValidationBehavior<,>));
```

### **Validation Flow**
1. **CreateUserCommand** received by dispatcher
2. **CommandValidationBehavior** intercepts command
3. **CreateUserCommandValidator** validates command properties
4. **ValidationException** thrown if validation fails
5. **Command proceeds** to handler if validation passes

## Error Response Format

### **Validation Failure Response**
When validation fails, the API returns a structured error response:

```json
{
  "title": "One or more validation errors occurred",
  "status": 400,
  "errors": {
    "GraphId": ["Graph ID must be a valid GUID format."],
    "UserId": ["User ID can only contain letters, numbers, dots, underscores, and hyphens."],
    "DisplayName": ["Display name is required."],
    "PrincipalName": ["Principal name must be a valid email address format."]
  }
}
```

## Business Rules Enforced

### **Uniqueness Constraints**
The validator enforces format rules, but uniqueness is handled at the database level:
- **GraphId**: Must be unique (database unique index)
- **UserId**: Must be unique (database unique index)  
- **PrincipalName**: Must be unique (database unique index)

### **Integration Compatibility**
- **Microsoft Graph**: GraphId format ensures Graph API compatibility
- **Active Directory**: PrincipalName format supports UPN lookups
- **External Systems**: UserId format supports cross-system integration

## Testing Scenarios

### **Valid Command Examples**
```csharp
// Valid command
var validCommand = new CreateUserCommand(
    GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    UserId: "john.doe",
    DisplayName: "John Doe",
    PrincipalName: "john.doe@company.com",
    IsEnabled: true
);
```

### **Invalid Command Examples**
```csharp
// Invalid GraphId format
var invalidGraphId = new CreateUserCommand(
    GraphId: "invalid-guid",  // ❌ Not a valid GUID
    UserId: "john.doe",
    DisplayName: "John Doe",
    PrincipalName: "john.doe@company.com"
);

// Invalid UserId format
var invalidUserId = new CreateUserCommand(
    GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    UserId: "john doe",  // ❌ Contains space
    DisplayName: "John Doe",
    PrincipalName: "john.doe@company.com"
);

// Invalid PrincipalName format
var invalidPrincipal = new CreateUserCommand(
    GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    UserId: "john.doe",
    DisplayName: "John Doe",
    PrincipalName: "not-an-email"  // ❌ Not email format
);
```

## Integration Points

### **Command Handler Integration**
The validator runs before the command handler, ensuring only valid data reaches the handler:

```csharp
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<User>>
{
    // Handler receives only validated commands
    public async Task<Result<User>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // command.GraphId, command.UserId, etc. are guaranteed to be valid
    }
}
```

### **API Endpoint Integration**
API endpoints automatically benefit from validation:

```csharp
app.MapPost("/api/users", async (CreateUserCommand command, ICommandDispatcher dispatcher) =>
{
    // ValidationException automatically caught and converted to 400 Bad Request
    var result = await dispatcher.Dispatch<CreateUserCommand, Result<User>>(command);
    return result.IsSuccess ? Results.Created($"/api/users/{result.Value.Id}", result.Value) : result.ToProblemDetails();
});
```

## Best Practices Applied

1. **✅ Database Constraint Alignment**: Validation rules match database field constraints
2. **✅ Business Logic Validation**: Format rules enforce business requirements
3. **✅ Security**: Input sanitization through format restrictions
4. **✅ User Experience**: Clear, actionable error messages
5. **✅ Performance**: Fast validation without database calls
6. **✅ Maintainability**: Centralized validation logic

---
*Date: August 30, 2025*
*Implementation: CreateUserCommandValidator*
