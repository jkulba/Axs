using Application.AccessRequests.Commands;
using Application.AccessRequests.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.Tests.AccessRequests.Validators;

public class CreateAccessRequestCommandValidatorTests
{
    private readonly CreateAccessRequestCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_UserName_Is_Empty()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("User name is required.");
    }

    [Fact]
    public void Should_Have_Error_When_JobNumber_Is_Zero_Or_Negative()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "john.doe",
            JobNumber: 0, // Invalid - should be positive
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.JobNumber)
            .WithErrorMessage("Job number must be a valid positive integer.");
    }

    [Fact]
    public void Should_Have_Error_When_RequestCode_Is_Empty_Guid()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.Empty, // Invalid - should not be empty GUID
            UserName: "john.doe",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.RequestCode)
            .WithErrorMessage("Request code must be a valid GUID if provided.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_RequestCode_Is_Null()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: null, // Valid - null is allowed
            UserName: "john.doe",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.RequestCode);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid_With_RequestCode()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "john.doe",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid_Without_RequestCode()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: null,
            UserName: "john.doe",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_JobNumber_Is_Negative()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "john.doe",
            JobNumber: -123, // Invalid - negative number
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.JobNumber)
            .WithErrorMessage("Job number must be a valid positive integer.");
    }

    [Fact]
    public void Should_Have_Error_When_UserName_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: new string('a', 101), // Invalid - exceeds 100 characters
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("User name cannot exceed 100 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_UserName_Contains_Invalid_Characters()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "john doe!", // Invalid - contains space and exclamation
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("User name can only contain letters, numbers, dots, underscores, @ symbols, and hyphens.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_UtcCreatedAt_Is_Null()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "john.doe",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: null // Null should be allowed
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.UtcCreatedAt);
    }

    [Fact]
    public void Should_Have_Error_When_UtcCreatedAt_Is_Too_Far_In_Future()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "john.doe",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow.AddMinutes(10) // Too far in the future
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UtcCreatedAt)
            .WithErrorMessage("Created date must be a valid date and cannot be in the future (allowing 5 minutes for clock skew) if provided.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_UtcCreatedAt_Is_Within_Clock_Skew_Tolerance()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "john.doe",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow.AddMinutes(3) // Within 5-minute tolerance
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.UtcCreatedAt);
    }

    [Fact]
    public void Should_Have_Error_When_ActivityCode_Is_Empty()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "john.doe",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "",
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ActivityCode)
            .WithErrorMessage("Activity code is required.");
    }

    [Fact]
    public void Should_Have_Error_When_ActivityCode_Is_Too_Long()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "john.doe",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: new string('A', 51), // 51 characters, exceeds 50 limit
            Workstation: "WS001",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ActivityCode)
            .WithErrorMessage("Activity code cannot exceed 50 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Workstation_Is_Empty()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "john.doe",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: "",
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Workstation)
            .WithErrorMessage("Workstation is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Workstation_Is_Too_Long()
    {
        // Arrange
        var command = new CreateAccessRequestCommand(
            RequestCode: Guid.NewGuid(),
            UserName: "john.doe",
            JobNumber: 12345,
            CycleNumber: 1,
            ActivityCode: "TEST",
            Workstation: new string('W', 256), // 256 characters, exceeds 255 limit
            ApplicationName: "TestApp",
            UtcCreatedAt: DateTime.UtcNow
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Workstation)
            .WithErrorMessage("Workstation cannot exceed 255 characters.");
    }
}
