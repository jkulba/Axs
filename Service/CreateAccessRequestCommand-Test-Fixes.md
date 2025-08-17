# Test Fixes for CreateAccessRequestCommand Primary Constructor

## Summary

Fixed all tests in `CreateAccessRequestCommandValidatorTests.cs` to work with the updated `CreateAccessRequestCommand` that now uses a primary constructor pattern.

## Changes Made

### 1. Constructor Syntax Update
- **Before**: Object initializer syntax (`new CreateAccessRequestCommand { ... }`)
- **After**: Primary constructor syntax (`new CreateAccessRequestCommand(...)`)

### 2. Parameter Type Corrections
- **JobNumber**: Changed from `string` (e.g., `"12345"`) to `int` (e.g., `12345`)
- All tests now provide proper `int` values for `JobNumber`

### 3. Complete Parameter Lists
- All constructor calls now provide all required parameters:
  - `RequestCode` (Guid?)
  - `UserName` (string?)
  - `JobNumber` (int)
  - `CycleNumber` (int)
  - `ActivityCode` (string?)
  - `Workstation` (string?)
  - `ApplicationName` (string?)
  - `UtcCreatedAt` (DateTime)

### 4. Updated Test Cases

#### Existing Tests (Fixed):
- ✅ `Should_Have_Error_When_UserName_Is_Empty`
- ✅ `Should_Have_Error_When_JobNumber_Is_Zero_Or_Negative` (renamed and fixed)
- ✅ `Should_Have_Error_When_RequestCode_Is_Empty_Guid`
- ✅ `Should_Not_Have_Error_When_RequestCode_Is_Null`
- ✅ `Should_Not_Have_Error_When_Command_Is_Valid_With_RequestCode`
- ✅ `Should_Not_Have_Error_When_Command_Is_Valid_Without_RequestCode`

#### New Tests Added:
- ✅ `Should_Have_Error_When_JobNumber_Is_Negative` - Tests negative job numbers
- ✅ `Should_Have_Error_When_UserName_Exceeds_MaxLength` - Tests 101-character usernames
- ✅ `Should_Have_Error_When_UserName_Contains_Invalid_Characters` - Tests regex validation

## Test Results

All **9 tests** are now passing:

```
Passed!  - Failed: 0, Passed: 9, Skipped: 0, Total: 9
```

## Key Validation Rules Tested

| Property | Validation Rule | Test Coverage |
|----------|-----------------|---------------|
| UserName | Required, Max 100 chars, Regex pattern | ✅ Empty, ✅ Too long, ✅ Invalid chars |
| JobNumber | Must be positive integer | ✅ Zero, ✅ Negative |
| RequestCode | Optional, but not empty GUID if provided | ✅ Empty GUID, ✅ Null (valid) |
| UtcCreatedAt | Required, not too far in future | ✅ Implicit in valid tests |

## Usage Examples

The updated constructor calls now follow this pattern:

```csharp
var command = new CreateAccessRequestCommand(
    RequestCode: Guid.NewGuid(),          // or null
    UserName: "john.doe",
    JobNumber: 12345,                     // int, not string
    CycleNumber: 1,
    ActivityCode: "TEST",
    Workstation: "WS001",
    ApplicationName: "TestApp",
    UtcCreatedAt: DateTime.UtcNow
);
```

This ensures all tests work correctly with the new primary constructor record pattern.
