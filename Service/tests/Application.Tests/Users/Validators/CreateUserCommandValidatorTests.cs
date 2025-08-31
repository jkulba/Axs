using Application.Users.Commands;
using Application.Users.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.Tests.Users.Validators;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        _validator = new CreateUserCommandValidator();
    }

    #region GraphId Validation Tests

    [Fact]
    public void GraphId_Should_NotBeEmpty()
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: string.Empty,
            UserId: "user123",
            DisplayName: "Test User",
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.GraphId)
            .WithErrorMessage("Graph ID is required.");
    }

    [Fact]
    public void GraphId_Should_NotExceedMaxLength()
    {
        // Arrange
        var longGraphId = new string('a', 256); // 256 characters
        var command = new CreateUserCommand(
            GraphId: longGraphId,
            UserId: "user123",
            DisplayName: "Test User",
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.GraphId)
            .WithErrorMessage("Graph ID cannot exceed 255 characters.");
    }

    [Theory]
    [InlineData("invalid-guid")]
    [InlineData("12345678-1234-1234-1234-12345678901")]
    [InlineData("12345678-1234-1234-1234-12345678901z")]
    [InlineData("12345678-1234-1234-1234")]
    [InlineData("not-a-guid-at-all")]
    public void GraphId_Should_MatchValidGuidFormat(string invalidGraphId)
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: invalidGraphId,
            UserId: "user123",
            DisplayName: "Test User",
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.GraphId)
            .WithErrorMessage("Graph ID must be a valid GUID format.");
    }

    [Theory]
    [InlineData("12345678-1234-1234-1234-123456789012")]
    [InlineData("ABCDEF12-3456-7890-ABCD-EF1234567890")]
    [InlineData("abcdef12-3456-7890-abcd-ef1234567890")]
    [InlineData("12345678-ABCD-efab-1234-567890ABCDEF")]
    public void GraphId_Should_BeValid_WhenValidGuidFormat(string validGraphId)
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: validGraphId,
            UserId: "user123",
            DisplayName: "Test User",
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.GraphId);
    }

    #endregion

    #region UserId Validation Tests

    [Fact]
    public void UserId_Should_NotBeEmpty()
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: string.Empty,
            DisplayName: "Test User",
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("User ID is required.");
    }

    [Fact]
    public void UserId_Should_NotExceedMaxLength()
    {
        // Arrange
        var longUserId = new string('a', 256); // 256 characters
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: longUserId,
            DisplayName: "Test User",
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("User ID cannot exceed 255 characters.");
    }

    [Theory]
    [InlineData("user@domain")]
    [InlineData("user with spaces")]
    [InlineData("user#special")]
    [InlineData("user$money")]
    [InlineData("user%percent")]
    public void UserId_Should_OnlyAllowValidCharacters(string invalidUserId)
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: invalidUserId,
            DisplayName: "Test User",
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("User ID can only contain letters, numbers, dots, underscores, and hyphens.");
    }

    [Theory]
    [InlineData("user123")]
    [InlineData("user.name")]
    [InlineData("user_name")]
    [InlineData("user-name")]
    [InlineData("User123")]
    [InlineData("user123.name_test-final")]
    public void UserId_Should_BeValid_WhenValidCharacters(string validUserId)
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: validUserId,
            DisplayName: "Test User",
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    #endregion

    #region DisplayName Validation Tests

    [Fact]
    public void DisplayName_Should_NotBeEmpty()
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: "user123",
            DisplayName: string.Empty,
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DisplayName)
            .WithErrorMessage("Display name is required.");
    }

    [Fact]
    public void DisplayName_Should_NotExceedMaxLength()
    {
        // Arrange
        var longDisplayName = new string('a', 256); // 256 characters
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: "user123",
            DisplayName: longDisplayName,
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DisplayName)
            .WithErrorMessage("Display name cannot exceed 255 characters.");
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData(" \t \n ")]
    public void DisplayName_Should_NotBeOnlyWhitespace(string whitespaceDisplayName)
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: "user123",
            DisplayName: whitespaceDisplayName,
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DisplayName)
            .WithErrorMessage("Display name cannot be only whitespace.");
    }

    [Theory]
    [InlineData("John Doe")]
    [InlineData("Jane Smith")]
    [InlineData("Test User 123")]
    [InlineData("User With Special Characters !@#$%")]
    public void DisplayName_Should_BeValid_WhenNotEmptyOrWhitespace(string validDisplayName)
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: "user123",
            DisplayName: validDisplayName,
            PrincipalName: "test@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.DisplayName);
    }

    #endregion

    #region PrincipalName Validation Tests

    [Fact]
    public void PrincipalName_Should_NotBeEmpty()
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: "user123",
            DisplayName: "Test User",
            PrincipalName: string.Empty,
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PrincipalName)
            .WithErrorMessage("Principal name is required.");
    }

    [Fact]
    public void PrincipalName_Should_NotExceedMaxLength()
    {
        // Arrange
        var longPrincipalName = new string('a', 250) + "@test.com"; // 260 characters total
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: "user123",
            DisplayName: "Test User",
            PrincipalName: longPrincipalName,
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PrincipalName)
            .WithErrorMessage("Principal name cannot exceed 255 characters.");
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    [InlineData("user.domain.com")]
    [InlineData("user@@domain.com")]
    public void PrincipalName_Should_BeValidEmailFormat(string invalidEmail)
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: "user123",
            DisplayName: "Test User",
            PrincipalName: invalidEmail,
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PrincipalName)
            .WithErrorMessage("Principal name must be a valid email address format.");
    }

    [Theory]
    [InlineData("user@domain.com")]
    [InlineData("test.user@example.org")]
    [InlineData("user123@test-domain.co.uk")]
    [InlineData("firstname.lastname@company.com")]
    public void PrincipalName_Should_BeValid_WhenValidEmailFormat(string validEmail)
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: "user123",
            DisplayName: "Test User",
            PrincipalName: validEmail,
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.PrincipalName);
    }

    #endregion

    #region IsEnabled Validation Tests

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsEnabled_Should_BeValid_WhenNotNull(bool isEnabled)
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: "user123",
            DisplayName: "Test User",
            PrincipalName: "test@example.com",
            IsEnabled: isEnabled
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.IsEnabled);
    }

    #endregion

    #region Complete Valid Command Test

    [Fact]
    public void CreateUserCommand_Should_BeValid_WhenAllPropertiesValid()
    {
        // Arrange
        var command = new CreateUserCommand(
            GraphId: "12345678-1234-1234-1234-123456789012",
            UserId: "user123",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@example.com",
            IsEnabled: true
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion
}
