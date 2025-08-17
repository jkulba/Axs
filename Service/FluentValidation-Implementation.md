# FluentValidation Implementation for VerifyAccessRequest

## Summary

This implementation adds FluentValidation to the RESTful service to validate the incoming `VerifyAccessRequest` immediately when it's received.

## Changes Made

### 1. Package Dependencies
- Added `FluentValidation` and `FluentValidation.DependencyInjectionExtensions` to `src/Api/Api.csproj`
- Added `FluentValidation.TestHelper` to `Directory.Packages.props` for testing support

### 2. Validation Configuration
- Modified `src/Api/Program.cs` to:
  - Import FluentValidation namespace
  - Register validators using `builder.Services.AddValidatorsFromAssemblyContaining<Program>()`
  - Added the authorization endpoint mapping `app.MapAuthorizationEndpoints()`

### 3. Validator Implementation
- Created `src/Api/Validators/VerifyAccessRequestValidator.cs` with validation rules:
  - `ProfileUserName`: Required, max 100 characters
  - `JobNumber`: Must be greater than 0
  - `CycleNumber`: Must be >= 0
  - `Workstation`: Required, max 50 characters
  - `ApplicationName`: Required, max 100 characters

### 4. Endpoint Updates
- Updated `src/Api/Endpoints/AuthorizationEndpoints.cs` to:
  - Inject `IValidator<VerifyAccessRequest>` into the endpoint
  - Perform validation before processing the request
  - Return proper validation error responses using `Results.ValidationProblem()`
  - Map the request to the command object
  - Improved error logging with structured logging

### 5. Supporting Types
- Updated `src/Application/Authorization/Commands/VerifyAccessCommand.cs` to include all required properties
- Created `src/Api/Contracts/AccessVerificationResult.cs` for the response type

### 6. Testing
- Created `tests/Api.Tests/` project with comprehensive unit tests
- Implemented `VerifyAccessRequestValidatorTests.cs` with test cases for:
  - All validation rules (empty values, length limits, range constraints)
  - Success case with valid data
- Added the new test project to the solution file

## Validation Rules Summary

| Property | Validation Rules |
|----------|------------------|
| ProfileUserName | Required, Max 100 characters |
| JobNumber | Must be > 0 |
| CycleNumber | Must be >= 0 |
| Workstation | Required, Max 50 characters |
| ApplicationName | Required, Max 100 characters |

## Usage

When a request is made to `/api/authorization/verify-access`, the validation occurs immediately:

1. The request is automatically validated using FluentValidation
2. If validation fails, a 400 Bad Request response is returned with detailed field-level error messages
3. If validation passes, the request is processed through the command dispatcher
4. The response includes appropriate success/failure information

## Error Response Format

For validation errors, the API returns a standard problem details response with field-level validation errors:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "ProfileUserName": ["ProfileUserName is required."],
    "JobNumber": ["JobNumber must be greater than 0."]
  }
}
```

## Testing

Run the validator tests using:
```bash
dotnet test tests/Api.Tests/
```

This validates that all validation rules work correctly and provides confidence in the validation behavior.
