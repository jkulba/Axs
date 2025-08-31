using Domain.Common;
using Domain.Errors;
using Xunit;

namespace Domain.Tests.Errors;

public class UserErrorsTests
{
    [Fact]
    public void UsersNotFound_Should_Have_Correct_Code()
    {
        // Act
        var error = UserErrors.UsersNotFound;

        // Assert
        Assert.Equal("User.UsersNotFound", error.Code);
    }

    [Fact]
    public void UsersNotFound_Should_Have_Correct_Description()
    {
        // Act
        var error = UserErrors.UsersNotFound;

        // Assert
        Assert.Equal("No users found", error.Description);
    }

    [Fact]
    public void UsersNotFound_Should_Be_Same_Reference()
    {
        // Act
        var error1 = UserErrors.UsersNotFound;
        var error2 = UserErrors.UsersNotFound;

        // Assert
        Assert.Equal(error1, error2);
        // Note: Properties create new instances each time, so they are not the same reference
        Assert.NotSame(error1, error2);
    }

    [Fact]
    public void UsersNotFound_Should_Be_Error_Type()
    {
        // Act
        var error = UserErrors.UsersNotFound;

        // Assert
        Assert.IsType<Error>(error);
        Assert.NotNull(error);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123)]
    [InlineData(999)]
    [InlineData(0)]
    [InlineData(-1)]
    public void UserByIdNotFound_Should_Have_Correct_Code(int id)
    {
        // Act
        var error = UserErrors.UserByIdNotFound(id);

        // Assert
        Assert.Equal("User.UserByIdNotFound", error.Code);
    }

    [Theory]
    [InlineData(1, "User with ID = '1' was not found")]
    [InlineData(123, "User with ID = '123' was not found")]
    [InlineData(999, "User with ID = '999' was not found")]
    [InlineData(0, "User with ID = '0' was not found")]
    [InlineData(-1, "User with ID = '-1' was not found")]
    public void UserByIdNotFound_Should_Have_Correct_Description(int id, string expectedDescription)
    {
        // Act
        var error = UserErrors.UserByIdNotFound(id);

        // Assert
        Assert.Equal(expectedDescription, error.Description);
    }

    [Fact]
    public void UserByIdNotFound_Should_Create_New_Instance_Each_Time()
    {
        // Arrange
        var id = 123;

        // Act
        var error1 = UserErrors.UserByIdNotFound(id);
        var error2 = UserErrors.UserByIdNotFound(id);

        // Assert
        Assert.Equal(error1, error2); // Same values
        Assert.NotSame(error1, error2); // Different instances
    }

    [Fact]
    public void UserByIdNotFound_Should_Be_Error_Type()
    {
        // Act
        var error = UserErrors.UserByIdNotFound(123);

        // Assert
        Assert.IsType<Error>(error);
        Assert.NotNull(error);
    }

    [Theory]
    [InlineData("john.doe")]
    [InlineData("user123")]
    [InlineData("test-user")]
    [InlineData("admin")]
    [InlineData("")]
    [InlineData("user.with.dots")]
    public void UserByUserIdNotFound_Should_Have_Correct_Code(string userId)
    {
        // Act
        var error = UserErrors.UserByUserIdNotFound(userId);

        // Assert
        Assert.Equal("User.UserByUserIdNotFound", error.Code);
    }

    [Theory]
    [InlineData("john.doe", "User with UserId = 'john.doe' was not found")]
    [InlineData("user123", "User with UserId = 'user123' was not found")]
    [InlineData("test-user", "User with UserId = 'test-user' was not found")]
    [InlineData("admin", "User with UserId = 'admin' was not found")]
    [InlineData("", "User with UserId = '' was not found")]
    [InlineData("user.with.dots", "User with UserId = 'user.with.dots' was not found")]
    public void UserByUserIdNotFound_Should_Have_Correct_Description(string userId, string expectedDescription)
    {
        // Act
        var error = UserErrors.UserByUserIdNotFound(userId);

        // Assert
        Assert.Equal(expectedDescription, error.Description);
    }

    [Fact]
    public void UserByUserIdNotFound_Should_Create_New_Instance_Each_Time()
    {
        // Arrange
        var userId = "test.user";

        // Act
        var error1 = UserErrors.UserByUserIdNotFound(userId);
        var error2 = UserErrors.UserByUserIdNotFound(userId);

        // Assert
        Assert.Equal(error1, error2); // Same values
        Assert.NotSame(error1, error2); // Different instances
    }

    [Fact]
    public void UserByUserIdNotFound_Should_Be_Error_Type()
    {
        // Act
        var error = UserErrors.UserByUserIdNotFound("test.user");

        // Assert
        Assert.IsType<Error>(error);
        Assert.NotNull(error);
    }

    [Fact]
    public void UserByUserIdNotFound_Should_Handle_Null_UserId()
    {
        // Act
        var error = UserErrors.UserByUserIdNotFound(null!);

        // Assert
        Assert.Equal("User.UserByUserIdNotFound", error.Code);
        Assert.Contains("User with UserId = ''", error.Description);
        Assert.NotNull(error);
    }

    [Fact]
    public void UserByIdNotFound_Should_Handle_Different_Ids()
    {
        // Act
        var error1 = UserErrors.UserByIdNotFound(1);
        var error2 = UserErrors.UserByIdNotFound(2);

        // Assert
        Assert.NotEqual(error1, error2);
        Assert.Equal("User.UserByIdNotFound", error1.Code);
        Assert.Equal("User.UserByIdNotFound", error2.Code);
        Assert.Contains("'1'", error1.Description);
        Assert.Contains("'2'", error2.Description);
    }

    [Fact]
    public void UserByUserIdNotFound_Should_Handle_Different_UserIds()
    {
        // Act
        var error1 = UserErrors.UserByUserIdNotFound("user1");
        var error2 = UserErrors.UserByUserIdNotFound("user2");

        // Assert
        Assert.NotEqual(error1, error2);
        Assert.Equal("User.UserByUserIdNotFound", error1.Code);
        Assert.Equal("User.UserByUserIdNotFound", error2.Code);
        Assert.Contains("'user1'", error1.Description);
        Assert.Contains("'user2'", error2.Description);
    }

    [Fact]
    public void UserByIdNotFound_Should_Include_Id_In_Description()
    {
        // Arrange
        var id = 42;

        // Act
        var error = UserErrors.UserByIdNotFound(id);

        // Assert
        Assert.Contains(id.ToString(), error.Description);
        Assert.Contains("User with ID", error.Description);
        Assert.Contains("was not found", error.Description);
    }

    [Fact]
    public void UserByUserIdNotFound_Should_Include_UserId_In_Description()
    {
        // Arrange
        var userId = "test.user.42";

        // Act
        var error = UserErrors.UserByUserIdNotFound(userId);

        // Assert
        Assert.Contains(userId, error.Description);
        Assert.Contains("User with UserId", error.Description);
        Assert.Contains("was not found", error.Description);
    }

    [Fact]
    public void UserErrors_Should_Have_Consistent_Code_Prefix()
    {
        // Act
        var staticError = UserErrors.UsersNotFound;
        var idError = UserErrors.UserByIdNotFound(1);
        var userIdError = UserErrors.UserByUserIdNotFound("test");

        // Assert
        Assert.StartsWith("User.", staticError.Code);
        Assert.StartsWith("User.", idError.Code);
        Assert.StartsWith("User.", userIdError.Code);
    }

    [Fact]
    public void UserErrors_Should_Have_Non_Empty_Descriptions()
    {
        // Act
        var staticError = UserErrors.UsersNotFound;
        var idError = UserErrors.UserByIdNotFound(1);
        var userIdError = UserErrors.UserByUserIdNotFound("test");

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(staticError.Description));
        Assert.False(string.IsNullOrWhiteSpace(idError.Description));
        Assert.False(string.IsNullOrWhiteSpace(userIdError.Description));
    }

    [Fact]
    public void UserErrors_Should_Have_Unique_Error_Codes()
    {
        // Act
        var staticError = UserErrors.UsersNotFound;
        var idError = UserErrors.UserByIdNotFound(1);
        var userIdError = UserErrors.UserByUserIdNotFound("test");

        // Assert
        Assert.NotEqual(staticError.Code, idError.Code);
        Assert.NotEqual(staticError.Code, userIdError.Code);
        Assert.NotEqual(idError.Code, userIdError.Code);
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void UserByIdNotFound_Should_Handle_Extreme_Values(int id)
    {
        // Act
        var error = UserErrors.UserByIdNotFound(id);

        // Assert
        Assert.Equal("User.UserByIdNotFound", error.Code);
        Assert.Contains(id.ToString(), error.Description);
        Assert.NotNull(error.Description);
        Assert.NotEmpty(error.Description);
    }

    [Theory]
    [InlineData("very.long.user.id.with.many.dots.and.characters.that.could.potentially.cause.issues")]
    [InlineData("user_with_underscores_and_123_numbers")]
    [InlineData("user-with-dashes-and-special-chars")]
    [InlineData("简体中文")] // Chinese characters
    [InlineData("한국어")] // Korean characters
    [InlineData("العربية")] // Arabic characters
    public void UserByUserIdNotFound_Should_Handle_Various_String_Formats(string userId)
    {
        // Act
        var error = UserErrors.UserByUserIdNotFound(userId);

        // Assert
        Assert.Equal("User.UserByUserIdNotFound", error.Code);
        Assert.Contains(userId, error.Description);
        Assert.NotNull(error.Description);
        Assert.NotEmpty(error.Description);
    }

    [Fact]
    public void UserErrors_Methods_Should_Return_Different_Types_Of_Errors()
    {
        // Act
        var staticError = UserErrors.UsersNotFound;
        var idError = UserErrors.UserByIdNotFound(1);
        var userIdError = UserErrors.UserByUserIdNotFound("test");

        // Assert
        // All are Error types but with different purposes
        Assert.IsType<Error>(staticError);
        Assert.IsType<Error>(idError);
        Assert.IsType<Error>(userIdError);
        
        // Different error codes indicate different types of errors
        Assert.Contains("UsersNotFound", staticError.Code);
        Assert.Contains("UserByIdNotFound", idError.Code);
        Assert.Contains("UserByUserIdNotFound", userIdError.Code);
    }
}
