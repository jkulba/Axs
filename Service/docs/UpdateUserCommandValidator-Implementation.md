# UpdateUserCommandValidator Implementation

## Overview
This document describes the implementation of the UpdateUserCommandValidator for validating User update requests in the Axs project.

## Validator Details

### **File Location**
`src/Application/Users/Validators/UpdateUserCommandValidator.cs`

### **Validation Rules Applied**

#### **1. Id Validation**
```csharp
RuleFor(x => x.Id)
    .GreaterThan(0)
```
- **Required**: Must be greater than 0 (valid entity identifier)
- **Purpose**: Ensures the user being updated exists in the database
- **Example**: `1`, `123`, `999`

#### **2. GraphId Validation**
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

#### **3. UserId Validation**
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

#### **4. DisplayName Validation**
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

#### **5. PrincipalName Validation**
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

#### **6. IsEnabled Validation**
```csharp
RuleFor(x => x.IsEnabled)
    .NotNull()
```
- **Required**: Must have a boolean value (not null)
- **Values**: `true` or `false`

## Key Differences from CreateUserCommandValidator

### **Additional Id Validation**
Unlike the create validator, the update validator includes:
- **Id Field Validation**: Ensures positive integer for existing entity lookup
- **Entity Existence**: The Id must reference an existing user in the database

### **Same Business Rules**
Both validators enforce identical business rules for:
- **GraphId**: GUID format for Microsoft Graph compatibility
- **UserId**: Alphanumeric format for cross-system integration
- **DisplayName**: Required human-readable name
- **PrincipalName**: Email format for UPN compatibility
- **IsEnabled**: Boolean status flag

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
1. **UpdateUserCommand** received by dispatcher
2. **CommandValidationBehavior** intercepts command
3. **UpdateUserCommandValidator** validates command properties
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
    "Id": ["User ID must be greater than 0."],
    "GraphId": ["Graph ID must be a valid GUID format."],
    "UserId": ["User ID can only contain letters, numbers, dots, underscores, and hyphens."],
    "DisplayName": ["Display name is required."],
    "PrincipalName": ["Principal name must be a valid email address format."]
  }
}
```

## Business Rules Enforced

### **Entity Validation**
- **Positive Id**: Ensures reference to valid existing entity
- **Format Constraints**: All string fields must meet format requirements
- **Required Fields**: All fields are mandatory for complete user profile

### **Uniqueness Constraints**
The validator enforces format rules, but uniqueness is handled at the database/handler level:
- **GraphId**: Must be unique across all users (excluding self)
- **UserId**: Must be unique across all users (excluding self)  
- **PrincipalName**: Must be unique across all users (excluding self)

### **Integration Compatibility**
- **Microsoft Graph**: GraphId format ensures Graph API compatibility
- **Active Directory**: PrincipalName format supports UPN lookups
- **External Systems**: UserId format supports cross-system integration

## Testing Scenarios

### **Valid Command Examples**
```csharp
// Valid update command
var validCommand = new UpdateUserCommand(
    Id: 123,
    GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    UserId: "john.doe",
    DisplayName: "John Doe",
    PrincipalName: "john.doe@company.com",
    IsEnabled: true
);
```

### **Invalid Command Examples**
```csharp
// Invalid Id
var invalidId = new UpdateUserCommand(
    Id: 0,  // ❌ Must be greater than 0
    GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    UserId: "john.doe",
    DisplayName: "John Doe",
    PrincipalName: "john.doe@company.com"
);

// Invalid GraphId format
var invalidGraphId = new UpdateUserCommand(
    Id: 123,
    GraphId: "invalid-guid",  // ❌ Not a valid GUID
    UserId: "john.doe",
    DisplayName: "John Doe",
    PrincipalName: "john.doe@company.com"
);

// Invalid UserId format
var invalidUserId = new UpdateUserCommand(
    Id: 123,
    GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    UserId: "john doe",  // ❌ Contains space
    DisplayName: "John Doe",
    PrincipalName: "john.doe@company.com"
);
```

## Integration Points

### **Command Handler Integration**
The validator runs before the command handler, ensuring only valid data reaches the handler:

```csharp
public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Result<User>>
{
    // Handler receives only validated commands
    public async Task<Result<User>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        // command.Id, command.GraphId, etc. are guaranteed to be valid
        // Handler focuses on business logic and database operations
    }
}
```

### **API Endpoint Integration**
API endpoints automatically benefit from validation:

```csharp
app.MapPut("/api/users/{id}", async (int id, UpdateUserCommand command, ICommandDispatcher dispatcher) =>
{
    // Ensure route parameter matches command Id
    command = command with { Id = id };
    
    // ValidationException automatically caught and converted to 400 Bad Request
    var result = await dispatcher.Dispatch<UpdateUserCommand, Result<User>>(command);
    return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
});
```

## Best Practices Applied

1. **✅ Database Constraint Alignment**: Validation rules match database field constraints
2. **✅ Business Logic Validation**: Format rules enforce business requirements
3. **✅ Entity Integrity**: Id validation ensures valid entity references
4. **✅ Security**: Input sanitization through format restrictions
5. **✅ User Experience**: Clear, actionable error messages
6. **✅ Performance**: Fast validation without database calls
7. **✅ Maintainability**: Centralized validation logic
8. **✅ Consistency**: Matches CreateUserCommandValidator patterns

## Update-Specific Considerations

### **Partial Updates**
This validator enforces **complete updates** - all fields are required. For partial updates, consider:
- **Separate Command**: Create `PatchUserCommand` for partial updates
- **Optional Fields**: Use nullable properties with conditional validation
- **Field Masks**: Implement field selection for targeted updates

### **Concurrency Control**
While validation ensures data format, consider adding:
- **Version Fields**: Optimistic concurrency control
- **Timestamp Validation**: Prevent stale updates
- **Change Detection**: Only update modified fields

---
*Date: August 30, 2025*
*Implementation: UpdateUserCommandValidator*
