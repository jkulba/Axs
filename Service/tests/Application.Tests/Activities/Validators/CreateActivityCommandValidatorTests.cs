using Application.Activities.Commands;
using Application.Activities.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.Tests.Activities.Validators;

public class CreateActivityCommandValidatorTests
{
    private readonly CreateActivityCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_ActivityCode_Is_Empty()
    {
        var command = new CreateActivityCommand(
            ActivityCode: "",
            ActivityName: "Test Activity",
            Description: "Test Description"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ActivityCode)
            .WithErrorMessage("Activity code is required.");
    }

    [Fact]
    public void Should_Have_Error_When_ActivityCode_Exceeds_MaxLength()
    {
        var command = new CreateActivityCommand(
            ActivityCode: new string('a', 51), // 51 characters
            ActivityName: "Test Activity",
            Description: "Test Description"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ActivityCode)
            .WithErrorMessage("Activity code cannot exceed 50 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_ActivityCode_Contains_Invalid_Characters()
    {
        var command = new CreateActivityCommand(
            ActivityCode: "invalid!@#$%",
            ActivityName: "Test Activity",
            Description: "Test Description"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ActivityCode)
            .WithErrorMessage("Activity code can only contain letters, numbers, dots, underscores, and hyphens.");
    }

    [Fact]
    public void Should_Have_Error_When_ActivityName_Is_Empty()
    {
        var command = new CreateActivityCommand(
            ActivityCode: "ACT001",
            ActivityName: "",
            Description: "Test Description"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ActivityName)
            .WithErrorMessage("Activity name is required.");
    }

    [Fact]
    public void Should_Have_Error_When_ActivityName_Exceeds_MaxLength()
    {
        var command = new CreateActivityCommand(
            ActivityCode: "ACT001",
            ActivityName: new string('a', 256), // 256 characters
            Description: "Test Description"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ActivityName)
            .WithErrorMessage("Activity name cannot exceed 255 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_MaxLength()
    {
        var command = new CreateActivityCommand(
            ActivityCode: "ACT001",
            ActivityName: "Test Activity",
            Description: new string('a', 1001) // 1001 characters
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description cannot exceed 1000 characters.");
    }

    [Fact]
    public void Should_Pass_Validation_When_All_Properties_Are_Valid()
    {
        var command = new CreateActivityCommand(
            ActivityCode: "ACT001",
            ActivityName: "Test Activity",
            Description: "Test Description"
        );

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Pass_Validation_When_Description_Is_Null()
    {
        var command = new CreateActivityCommand(
            ActivityCode: "ACT001",
            ActivityName: "Test Activity",
            Description: null
        );

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Pass_Validation_When_ActivityCode_Contains_Valid_Characters()
    {
        var command = new CreateActivityCommand(
            ActivityCode: "ACT-001.TEST_V2",
            ActivityName: "Test Activity",
            Description: "Test Description"
        );

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
