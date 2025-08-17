using Api.Contracts;
using Api.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Api.Tests.Validators;

public class VerifyAccessRequestValidatorTests
{
    private readonly VerifyAccessRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_ProfileUserName_Is_Empty()
    {
        var request = new VerifyAccessRequest
        {
            ProfileUserName = "",
            JobNumber = 1,
            CycleNumber = 0,
            Workstation = "WS001",
            ApplicationName = "TestApp"
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.ProfileUserName)
            .WithErrorMessage("ProfileUserName is required.");
    }

    [Fact]
    public void Should_Have_Error_When_ProfileUserName_Exceeds_MaxLength()
    {
        var request = new VerifyAccessRequest
        {
            ProfileUserName = new string('a', 101), // 101 characters
            JobNumber = 1,
            CycleNumber = 0,
            Workstation = "WS001",
            ApplicationName = "TestApp"
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.ProfileUserName)
            .WithErrorMessage("ProfileUserName cannot exceed 100 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_JobNumber_Is_Zero_Or_Negative()
    {
        var request = new VerifyAccessRequest
        {
            ProfileUserName = "testuser",
            JobNumber = 0,
            CycleNumber = 0,
            Workstation = "WS001",
            ApplicationName = "TestApp"
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.JobNumber)
            .WithErrorMessage("JobNumber must be greater than 0.");
    }

    [Fact]
    public void Should_Have_Error_When_CycleNumber_Is_Negative()
    {
        var request = new VerifyAccessRequest
        {
            ProfileUserName = "testuser",
            JobNumber = 1,
            CycleNumber = -1,
            Workstation = "WS001",
            ApplicationName = "TestApp"
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.CycleNumber)
            .WithErrorMessage("CycleNumber must be greater than or equal to 0.");
    }

    [Fact]
    public void Should_Have_Error_When_Workstation_Is_Empty()
    {
        var request = new VerifyAccessRequest
        {
            ProfileUserName = "testuser",
            JobNumber = 1,
            CycleNumber = 0,
            Workstation = "",
            ApplicationName = "TestApp"
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Workstation)
            .WithErrorMessage("Workstation is required.");
    }

    [Fact]
    public void Should_Have_Error_When_ApplicationName_Is_Empty()
    {
        var request = new VerifyAccessRequest
        {
            ProfileUserName = "testuser",
            JobNumber = 1,
            CycleNumber = 0,
            Workstation = "WS001",
            ApplicationName = ""
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.ApplicationName)
            .WithErrorMessage("ApplicationName is required.");
    }

    [Fact]
    public void Should_Pass_Validation_When_All_Properties_Are_Valid()
    {
        var request = new VerifyAccessRequest
        {
            ProfileUserName = "testuser",
            JobNumber = 12345,
            CycleNumber = 1,
            Workstation = "WS001",
            ApplicationName = "TestApp"
        };

        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
