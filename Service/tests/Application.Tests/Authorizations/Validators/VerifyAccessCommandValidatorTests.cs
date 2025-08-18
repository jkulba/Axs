using Application.Authorization.Commands;
using Application.Authorization.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.Tests.Authorizations.Validators;

public class VerifyAccessCommandValidatorTests
{
    private readonly VerifyAccessCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_ProfileUserName_Is_Empty()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: "",
            JobNumber: 1,
            CycleNumber: 0,
            Workstation: "WS001",
            ApplicationName: "TestApp",
            ActivityCode: "ACT001"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ProfileUserName)
            .WithErrorMessage("ProfileUserName is required.");
    }

    [Fact]
    public void Should_Have_Error_When_ProfileUserName_Exceeds_MaxLength()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: new string('a', 101), // 101 characters
            JobNumber: 1,
            CycleNumber: 0,
            Workstation: "WS001",
            ApplicationName: "TestApp",
            ActivityCode: "ACT001"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ProfileUserName)
            .WithErrorMessage("ProfileUserName cannot exceed 100 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_JobNumber_Is_Zero_Or_Negative()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: "testuser",
            JobNumber: 0,
            CycleNumber: 0,
            Workstation: "WS001",
            ApplicationName: "TestApp",
            ActivityCode: "ACT001"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.JobNumber)
            .WithErrorMessage("JobNumber must be greater than 0.");
    }

    [Fact]
    public void Should_Have_Error_When_CycleNumber_Is_Negative()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: "testuser",
            JobNumber: 1,
            CycleNumber: -1,
            Workstation: "WS001",
            ApplicationName: "TestApp",
            ActivityCode: "ACT001"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CycleNumber)
            .WithErrorMessage("CycleNumber must be greater than or equal to 0.");
    }

    [Fact]
    public void Should_Have_Error_When_Workstation_Is_Empty()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: "testuser",
            JobNumber: 1,
            CycleNumber: 0,
            Workstation: "",
            ApplicationName: "TestApp",
            ActivityCode: "ACT001"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Workstation)
            .WithErrorMessage("Workstation is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Workstation_Exceeds_MaxLength()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: "testuser",
            JobNumber: 1,
            CycleNumber: 0,
            Workstation: new string('a', 51), // 51 characters
            ApplicationName: "TestApp",
            ActivityCode: "ACT001"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Workstation)
            .WithErrorMessage("Workstation cannot exceed 50 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_ApplicationName_Is_Empty()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: "testuser",
            JobNumber: 1,
            CycleNumber: 0,
            Workstation: "WS001",
            ApplicationName: "",
            ActivityCode: "ACT001"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ApplicationName)
            .WithErrorMessage("ApplicationName is required.");
    }

    [Fact]
    public void Should_Have_Error_When_ApplicationName_Exceeds_MaxLength()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: "testuser",
            JobNumber: 1,
            CycleNumber: 0,
            Workstation: "WS001",
            ApplicationName: new string('a', 101), // 101 characters
            ActivityCode: "ACT001"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ApplicationName)
            .WithErrorMessage("ApplicationName cannot exceed 100 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_ActivityCode_Exceeds_MaxLength()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: "testuser",
            JobNumber: 1,
            CycleNumber: 0,
            Workstation: "WS001",
            ApplicationName: "TestApp",
            ActivityCode: new string('a', 51) // 51 characters
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ActivityCode)
            .WithErrorMessage("ActivityCode cannot exceed 50 characters when provided.");
    }

    [Fact]
    public void Should_Pass_Validation_When_All_Properties_Are_Valid()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: "testuser",
            JobNumber: 12345,
            CycleNumber: 1,
            Workstation: "WS001",
            ApplicationName: "TestApp",
            ActivityCode: "ACT001"
        );

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Pass_Validation_When_ActivityCode_Is_Null()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: "testuser",
            JobNumber: 12345,
            CycleNumber: 1,
            Workstation: "WS001",
            ApplicationName: "TestApp",
            ActivityCode: null
        );

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Pass_Validation_When_ActivityCode_Is_Empty()
    {
        var command = new VerifyAccessCommand(
            ProfileUserName: "testuser",
            JobNumber: 12345,
            CycleNumber: 1,
            Workstation: "WS001",
            ApplicationName: "TestApp",
            ActivityCode: ""
        );

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
