using Application.Users.Commands;
using Xunit;

namespace Application.Tests.Users.Commands;

public class DeleteUserCommandTests
{
    [Fact]
    public void DeleteUserCommand_Should_Be_Record()
    {
        // Act & Assert
        Assert.True(typeof(DeleteUserCommand).IsAssignableTo(typeof(IEquatable<DeleteUserCommand>)));
    }

    [Fact]
    public void DeleteUserCommand_Should_Have_Required_Properties()
    {
        // Arrange
        var id = 123;

        // Act
        var command = new DeleteUserCommand(Id: id);

        // Assert
        Assert.Equal(id, command.Id);
    }

    [Fact]
    public void DeleteUserCommand_Should_Support_Equality_Comparison()
    {
        // Arrange
        var command1 = new DeleteUserCommand(Id: 123);
        var command2 = new DeleteUserCommand(Id: 123);

        // Act & Assert
        Assert.Equal(command1, command2);
        Assert.True(command1 == command2);
        Assert.False(command1 != command2);
    }

    [Fact]
    public void DeleteUserCommand_Should_Support_Inequality_Comparison()
    {
        // Arrange
        var command1 = new DeleteUserCommand(Id: 123);
        var command2 = new DeleteUserCommand(Id: 456);

        // Act & Assert
        Assert.NotEqual(command1, command2);
        Assert.False(command1 == command2);
        Assert.True(command1 != command2);
    }

    [Fact]
    public void DeleteUserCommand_Should_Have_Consistent_HashCode()
    {
        // Arrange
        var command1 = new DeleteUserCommand(Id: 123);
        var command2 = new DeleteUserCommand(Id: 123);

        // Act & Assert
        Assert.Equal(command1.GetHashCode(), command2.GetHashCode());
    }

    [Fact]
    public void DeleteUserCommand_Should_Support_With_Expression()
    {
        // Arrange
        var originalCommand = new DeleteUserCommand(Id: 123);
        var newId = 456;

        // Act
        var modifiedCommand = originalCommand with { Id = newId };

        // Assert
        Assert.Equal(newId, modifiedCommand.Id);
        Assert.NotEqual(originalCommand, modifiedCommand);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123)]
    [InlineData(999)]
    [InlineData(int.MaxValue)]
    public void DeleteUserCommand_Should_Accept_Valid_Id_Values(int id)
    {
        // Act
        var command = new DeleteUserCommand(Id: id);

        // Assert
        Assert.Equal(id, command.Id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    [InlineData(int.MinValue)]
    public void DeleteUserCommand_Should_Accept_Edge_Case_Id_Values(int id)
    {
        // Act
        var command = new DeleteUserCommand(Id: id);

        // Assert
        Assert.Equal(id, command.Id);
    }

    [Fact]
    public void DeleteUserCommand_Should_Have_Meaningful_ToString()
    {
        // Arrange
        var command = new DeleteUserCommand(Id: 123);

        // Act
        var stringRepresentation = command.ToString();

        // Assert
        Assert.Contains("DeleteUserCommand", stringRepresentation);
        Assert.Contains("123", stringRepresentation);
    }

    [Fact]
    public void DeleteUserCommand_Should_Support_Property_Access()
    {
        // Arrange
        var command = new DeleteUserCommand(Id: 123);

        // Act
        var id = command.Id;

        // Assert
        Assert.Equal(123, id);
    }

    [Fact]
    public void DeleteUserCommand_Should_Be_Immutable()
    {
        // Arrange
        var command = new DeleteUserCommand(Id: 123);

        // Act & Assert
        // Records with positional parameters have init-only setters
        // This test verifies properties can only be set during initialization
        var properties = typeof(DeleteUserCommand).GetProperties();
        foreach (var property in properties)
        {
            var setter = property.GetSetMethod();
            if (setter != null)
            {
                // For records, setters should be init-only (not publicly accessible after construction)
                var modifiers = setter.ReturnParameter.GetRequiredCustomModifiers();
                Assert.Contains(modifiers, t => t.Name == "IsExternalInit");
            }
        }
    }

    [Fact]
    public void DeleteUserCommand_Should_Have_Single_Property()
    {
        // Act
        var properties = typeof(DeleteUserCommand).GetProperties();

        // Assert
        Assert.Single(properties);
        Assert.Equal("Id", properties[0].Name);
        Assert.Equal(typeof(int), properties[0].PropertyType);
    }

    [Fact]
    public void DeleteUserCommand_Should_Support_Value_Semantics()
    {
        // Arrange
        var command1 = new DeleteUserCommand(Id: 123);
        var command2 = new DeleteUserCommand(Id: 123);
        var command3 = new DeleteUserCommand(Id: 456);

        // Act & Assert
        // Value equality
        Assert.Equal(command1, command2);
        Assert.NotEqual(command1, command3);

        // Reference equality (should be different instances)
        Assert.False(ReferenceEquals(command1, command2));
        Assert.False(ReferenceEquals(command1, command3));

        // Hash code consistency
        Assert.Equal(command1.GetHashCode(), command2.GetHashCode());
        Assert.NotEqual(command1.GetHashCode(), command3.GetHashCode());
    }

    [Fact]
    public void DeleteUserCommand_Should_Be_Simple_Command()
    {
        // Arrange & Act
        var command = new DeleteUserCommand(Id: 123);

        // Assert
        // Verify it's a simple command with minimal structure
        Assert.NotNull(command);
        Assert.Equal(123, command.Id);
        Assert.IsType<DeleteUserCommand>(command);
    }

    [Fact]
    public void DeleteUserCommand_Should_Handle_Large_Id_Values()
    {
        // Arrange
        var largeId = int.MaxValue;

        // Act
        var command = new DeleteUserCommand(Id: largeId);

        // Assert
        Assert.Equal(largeId, command.Id);
        Assert.Contains(largeId.ToString(), command.ToString());
    }

    [Fact]
    public void DeleteUserCommand_Should_Handle_Negative_Id_Values()
    {
        // Arrange
        var negativeId = -123;

        // Act
        var command = new DeleteUserCommand(Id: negativeId);

        // Assert
        Assert.Equal(negativeId, command.Id);
        Assert.Contains(negativeId.ToString(), command.ToString());
    }

    [Fact]
    public void DeleteUserCommand_Should_Distinguish_From_Other_User_Commands()
    {
        // Arrange
        var deleteCommand = new DeleteUserCommand(Id: 123);
        var updateCommand = new UpdateUserCommand(
            Id: 123,
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com"
        );
        var createCommand = new CreateUserCommand(
            GraphId: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId: "john.doe",
            DisplayName: "John Doe",
            PrincipalName: "john.doe@company.com"
        );

        // Act & Assert
        Assert.NotEqual(deleteCommand.GetType(), updateCommand.GetType());
        Assert.NotEqual(deleteCommand.GetType(), createCommand.GetType());
        Assert.Contains("DeleteUserCommand", deleteCommand.ToString());
        Assert.Contains("UpdateUserCommand", updateCommand.ToString());
        Assert.Contains("CreateUserCommand", createCommand.ToString());
    }

    [Fact]
    public void DeleteUserCommand_Constructor_Should_Set_Id_Property()
    {
        // Arrange
        var expectedId = 999;

        // Act
        var command = new DeleteUserCommand(expectedId);

        // Assert
        Assert.Equal(expectedId, command.Id);
    }

    [Fact]
    public void DeleteUserCommand_Should_Support_Multiple_Instances_With_Different_Ids()
    {
        // Arrange & Act
        var command1 = new DeleteUserCommand(Id: 1);
        var command2 = new DeleteUserCommand(Id: 2);
        var command3 = new DeleteUserCommand(Id: 3);

        // Assert
        Assert.Equal(1, command1.Id);
        Assert.Equal(2, command2.Id);
        Assert.Equal(3, command3.Id);
        
        Assert.NotEqual(command1, command2);
        Assert.NotEqual(command2, command3);
        Assert.NotEqual(command1, command3);
    }
}
