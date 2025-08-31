using Application.Users.Commands;
using Xunit;

namespace Application.Tests.Users.Commands;

public class UpdateUserCommandTests
{
    [Fact]
    public void UpdateUserCommand_Should_Be_Record()
    {
        // Act & Assert
        Assert.True(typeof(UpdateUserCommand).IsAssignableTo(typeof(IEquatable<UpdateUserCommand>)));
    }

    [Fact]
    public void UpdateUserCommand_Should_Have_Required_Properties()
    {
        // Arrange
        var id = 123;
        var graphId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890";
        var userId = "john.doe";
        var displayName = "John Doe";
        var principalName = "john.doe@company.com";
        var isEnabled = false;

        // Act
        var command = new UpdateUserCommand(
            Id: id,
            GraphId: graphId,
            UserId: userId,
            DisplayName: displayName,
            PrincipalName: principalName,
            IsEnabled: isEnabled
        );

        // Assert
        Assert.Equal(id, command.Id);
        Assert.Equal(graphId, command.GraphId);
        Assert.Equal(userId, command.UserId);
        Assert.Equal(displayName, command.DisplayName);
        Assert.Equal(principalName, command.PrincipalName);
        Assert.Equal(isEnabled, command.IsEnabled);
    }

    [Fact]
    public void UpdateUserCommand_Should_Have_Default_IsEnabled_True()
    {
        // Act
        var command = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com"
        );

        // Assert
        Assert.True(command.IsEnabled);
    }

    [Fact]
    public void UpdateUserCommand_Should_Support_Equality_Comparison()
    {
        // Arrange
        var command1 = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        var command2 = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        // Act & Assert
        Assert.Equal(command1, command2);
        Assert.True(command1 == command2);
        Assert.False(command1 != command2);
    }

    [Fact]
    public void UpdateUserCommand_Should_Support_Inequality_Comparison()
    {
        // Arrange
        var command1 = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        var command2 = new UpdateUserCommand(
            Id: 456, // Different Id
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        // Act & Assert
        Assert.NotEqual(command1, command2);
        Assert.False(command1 == command2);
        Assert.True(command1 != command2);
    }

    [Fact]
    public void UpdateUserCommand_Should_Have_Consistent_HashCode()
    {
        // Arrange
        var command1 = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        var command2 = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        // Act & Assert
        Assert.Equal(command1.GetHashCode(), command2.GetHashCode());
    }

    [Fact]
    public void UpdateUserCommand_Should_Support_With_Expression_For_Id()
    {
        // Arrange
        var originalCommand = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        var newId = 456;

        // Act
        var modifiedCommand = originalCommand with { Id = newId };

        // Assert
        Assert.Equal(newId, modifiedCommand.Id);
        Assert.Equal(originalCommand.GraphId, modifiedCommand.GraphId);
        Assert.Equal(originalCommand.UserId, modifiedCommand.UserId);
        Assert.Equal(originalCommand.DisplayName, modifiedCommand.DisplayName);
        Assert.Equal(originalCommand.PrincipalName, modifiedCommand.PrincipalName);
        Assert.Equal(originalCommand.IsEnabled, modifiedCommand.IsEnabled);
        Assert.NotEqual(originalCommand, modifiedCommand);
    }

    [Fact]
    public void UpdateUserCommand_Should_Support_With_Expression_For_IsEnabled()
    {
        // Arrange
        var originalCommand = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        // Act
        var modifiedCommand = originalCommand with { IsEnabled = false };

        // Assert
        Assert.False(modifiedCommand.IsEnabled);
        Assert.Equal(originalCommand.Id, modifiedCommand.Id);
        Assert.Equal(originalCommand.GraphId, modifiedCommand.GraphId);
        Assert.Equal(originalCommand.UserId, modifiedCommand.UserId);
        Assert.Equal(originalCommand.DisplayName, modifiedCommand.DisplayName);
        Assert.Equal(originalCommand.PrincipalName, modifiedCommand.PrincipalName);
        Assert.NotEqual(originalCommand, modifiedCommand);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123)]
    [InlineData(999)]
    [InlineData(int.MaxValue)]
    public void UpdateUserCommand_Should_Accept_Valid_Id_Values(int id)
    {
        // Act
        var command = new UpdateUserCommand(
            Id: id,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "test.user",
            DisplayName: "Test User",
            PrincipalName: "test.user@company.com"
        );

        // Assert
        Assert.Equal(id, command.Id);
    }

    [Theory]
    [InlineData("a1b2c3d4-e5f6-7890-abcd-ef1234567890")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF")]
    public void UpdateUserCommand_Should_Accept_Valid_GraphId_Formats(string graphId)
    {
        // Act
        var command = new UpdateUserCommand(
            Id: 123,
            GraphId: graphId,
            UserId: "test.user",
            DisplayName: "Test User",
            PrincipalName: "test.user@company.com"
        );

        // Assert
        Assert.Equal(graphId, command.GraphId);
    }

    [Theory]
    [InlineData("john.doe")]
    [InlineData("user_123")]
    [InlineData("test-user")]
    [InlineData("simple")]
    [InlineData("user.with.dots")]
    [InlineData("user_with_underscores")]
    [InlineData("user-with-dashes")]
    [InlineData("user123")]
    public void UpdateUserCommand_Should_Accept_Valid_UserId_Formats(string userId)
    {
        // Act
        var command = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: userId,
            DisplayName: "Test User",
            PrincipalName: "test.user@company.com"
        );

        // Assert
        Assert.Equal(userId, command.UserId);
    }

    [Theory]
    [InlineData("john.doe@company.com")]
    [InlineData("user@domain.org")]
    [InlineData("test.user@example.net")]
    [InlineData("admin@organization.gov")]
    public void UpdateUserCommand_Should_Accept_Valid_PrincipalName_Formats(string principalName)
    {
        // Act
        var command = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "test.user",
            DisplayName: "Test User",
            PrincipalName: principalName
        );

        // Assert
        Assert.Equal(principalName, command.PrincipalName);
    }

    [Theory]
    [InlineData("John Doe")]
    [InlineData("Jane Smith")]
    [InlineData("Test User")]
    [InlineData("Administrator")]
    [InlineData("System Account")]
    public void UpdateUserCommand_Should_Accept_Valid_DisplayName_Formats(string displayName)
    {
        // Act
        var command = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "test.user",
            DisplayName: displayName,
            PrincipalName: "test.user@company.com"
        );

        // Assert
        Assert.Equal(displayName, command.DisplayName);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void UpdateUserCommand_Should_Accept_Both_IsEnabled_Values(bool isEnabled)
    {
        // Act
        var command = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "test.user",
            DisplayName: "Test User",
            PrincipalName: "test.user@company.com",
            IsEnabled: isEnabled
        );

        // Assert
        Assert.Equal(isEnabled, command.IsEnabled);
    }

    [Fact]
    public void UpdateUserCommand_Should_Have_Meaningful_ToString()
    {
        // Arrange
        var command = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        // Act
        var stringRepresentation = command.ToString();

        // Assert
        Assert.Contains("UpdateUserCommand", stringRepresentation);
        Assert.Contains("123", stringRepresentation);
        Assert.Contains("john.doe", stringRepresentation);
        Assert.Contains("John Doe", stringRepresentation);
    }

    [Fact]
    public void UpdateUserCommand_Should_Support_Deconstruction()
    {
        // Arrange
        var command = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        // Act
        var (id, graphId, userId, displayName, principalName, isEnabled) = command;

        // Assert
        Assert.Equal(123, id);
        Assert.Equal("a1b2c3d4-e5f6-7890-abcd-ef1234567890", graphId);
        Assert.Equal("john.doe", userId);
        Assert.Equal("John Doe", displayName);
        Assert.Equal("john.doe@company.com", principalName);
        Assert.True(isEnabled);
    }

    [Fact]
    public void UpdateUserCommand_Should_Support_Multiple_With_Expressions()
    {
        // Arrange
        var originalCommand = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        // Act
        var modifiedCommand = originalCommand with 
        { 
            Id = 456,
            DisplayName = "John Smith",
            PrincipalName = "john.smith@company.com",
            IsEnabled = false
        };

        // Assert
        Assert.Equal(456, modifiedCommand.Id);
        Assert.Equal(originalCommand.GraphId, modifiedCommand.GraphId);
        Assert.Equal(originalCommand.UserId, modifiedCommand.UserId);
        Assert.Equal("John Smith", modifiedCommand.DisplayName);
        Assert.Equal("john.smith@company.com", modifiedCommand.PrincipalName);
        Assert.False(modifiedCommand.IsEnabled);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    [InlineData(int.MinValue)]
    public void UpdateUserCommand_Should_Accept_Edge_Case_Id_Values(int id)
    {
        // Act
        var command = new UpdateUserCommand(
            Id: id,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "test.user",
            DisplayName: "Test User",
            PrincipalName: "test.user@company.com"
        );

        // Assert
        Assert.Equal(id, command.Id);
    }

    [Fact]
    public void UpdateUserCommand_Should_Distinguish_From_CreateUserCommand()
    {
        // Arrange
        var updateCommand = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        var createCommand = new CreateUserCommand(
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com",
            IsEnabled: true
        );

        // Act & Assert
        Assert.NotEqual(updateCommand.GetType(), createCommand.GetType());
        Assert.Contains("UpdateUserCommand", updateCommand.ToString());
        Assert.Contains("CreateUserCommand", createCommand.ToString());
    }
}
