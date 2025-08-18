using Domain.Common;
using Xunit;

namespace Domain.Tests.Common;

public class ResultTests
{
    [Fact]
    public void Success_Should_Create_Successful_Result()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsNotFound);
        Assert.Equal(string.Empty, result.ErrorMessage);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Failure_With_String_Should_Create_Failed_Result()
    {
        // Arrange
        var errorMessage = "Test error message";

        // Act
        var result = Result.Failure(errorMessage);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.False(result.IsNotFound);
        Assert.Equal(errorMessage, result.ErrorMessage);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Failure_With_Error_Should_Create_Failed_Result()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error description");

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.False(result.IsNotFound);
        Assert.Equal(error.ToString(), result.ErrorMessage);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void NotFound_With_Error_Should_Create_NotFound_Result()
    {
        // Arrange
        var error = new Error("NotFound.Resource", "Resource not found");

        // Act
        var result = Result.NotFound(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsNotFound);
        Assert.Equal(error.ToString(), result.ErrorMessage);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void NotFound_With_String_Should_Create_NotFound_Result()
    {
        // Arrange
        var resourceName = "TestResource";

        // Act
        var result = Result.NotFound(resourceName);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsNotFound);
        Assert.Contains("TestResource", result.Error.Code);
        Assert.Contains("TestResource", result.Error.Description);
    }

    [Fact]
    public void IsNotFound_Should_Return_True_For_NotFound_Error()
    {
        // Arrange
        var result = Result.Failure(Error.NotFound);

        // Act & Assert
        Assert.True(result.IsNotFound);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Success_With_Non_None_Error()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new TestableResult(true, error));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Failure_With_None_Error()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new TestableResult(false, Error.None));
    }

    [Fact]
    public void ToString_Should_Return_Success_For_Successful_Result()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.Equal("Success", result.ToString());
    }

    [Fact]
    public void ToString_Should_Return_Failure_Message_For_Failed_Result()
    {
        // Arrange
        var errorMessage = "Test error";
        var result = Result.Failure(errorMessage);

        // Act
        var toString = result.ToString();

        // Assert
        Assert.Equal($"Failure: {errorMessage}", toString);
    }
}

// Test helper class to access protected constructor
public class TestableResult : Result
{
    public TestableResult(bool isSuccess, Error error) : base(isSuccess, error)
    {
    }
}
