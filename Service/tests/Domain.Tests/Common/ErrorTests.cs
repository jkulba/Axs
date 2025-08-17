using Domain.Common;
using Xunit;

namespace Domain.Tests.Common;

public class ErrorTests
{
    [Fact]
    public void Constructor_Should_Set_Code_And_Description()
    {
        // Arrange
        var code = "Test.Error";
        var description = "Test error description";

        // Act
        var error = new Error(code, description);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(description, error.Description);
    }

    [Fact]
    public void None_Should_Have_Empty_Code_And_Description()
    {
        // Act
        var error = Error.None;

        // Assert
        Assert.Equal(string.Empty, error.Code);
        Assert.Equal(string.Empty, error.Description);
    }

    [Fact]
    public void NullValue_Should_Have_Correct_Code_And_Description()
    {
        // Act
        var error = Error.NullValue;

        // Assert
        Assert.Equal("Error.NullValue", error.Code);
        Assert.Equal("Null value was provided", error.Description);
    }

    [Fact]
    public void NotFound_Should_Have_Correct_Code_And_Description()
    {
        // Act
        var error = Error.NotFound;

        // Assert
        Assert.Equal("Error.NotFound", error.Code);
        Assert.Equal("The requested resource was not found", error.Description);
    }

    [Fact]
    public void Implicit_Conversion_To_Result_Should_Create_Failure()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error description");

        // Act
        Result result = error;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error.Description, result.ErrorMessage);
    }

    [Fact]
    public void ToResult_Should_Create_Failure_Result()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error description");

        // Act
        var result = error.ToResult();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(error.Description, result.ErrorMessage);
    }

    [Fact]
    public void ToString_Should_Return_Record_Format()
    {
        // Arrange
        var description = "Test error description";
        var error = new Error("Test.Error", description);

        // Act
        var toString = error.ToString();

        // Assert
        Assert.Contains("Test.Error", toString);
        Assert.Contains(description, toString);
    }

    [Fact]
    public void Equality_Should_Work_For_Same_Values()
    {
        // Arrange
        var error1 = new Error("Test.Error", "Test description");
        var error2 = new Error("Test.Error", "Test description");

        // Act & Assert
        Assert.Equal(error1, error2);
        Assert.True(error1 == error2);
        Assert.False(error1 != error2);
    }

    [Fact]
    public void Equality_Should_Fail_For_Different_Values()
    {
        // Arrange
        var error1 = new Error("Test.Error1", "Test description");
        var error2 = new Error("Test.Error2", "Test description");

        // Act & Assert
        Assert.NotEqual(error1, error2);
        Assert.False(error1 == error2);
        Assert.True(error1 != error2);
    }

    [Fact]
    public void GetHashCode_Should_Be_Same_For_Equal_Errors()
    {
        // Arrange
        var error1 = new Error("Test.Error", "Test description");
        var error2 = new Error("Test.Error", "Test description");

        // Act & Assert
        Assert.Equal(error1.GetHashCode(), error2.GetHashCode());
    }

    [Fact]
    public void Static_Errors_Should_Be_Consistent()
    {
        // Assert
        Assert.Same(Error.None, Error.None);
        Assert.Same(Error.NullValue, Error.NullValue);
        Assert.Same(Error.NotFound, Error.NotFound);
    }
}
