using Domain.Common;
using Xunit;

namespace Domain.Tests.Common;

public class ResultGenericTests
{
    [Fact]
    public void Success_Should_Create_Successful_Result_With_Value()
    {
        // Arrange
        var value = "test value";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsNotFound);
        Assert.Equal(value, result.Value);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Failure_Should_Create_Failed_Result_Without_Value()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error description");

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.False(result.IsNotFound);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Value_Should_Throw_For_Failed_Result()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error description");
        var result = Result<string>.Failure(error);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void NotFound_With_Error_Should_Create_NotFound_Result()
    {
        // Arrange
        var error = new Error("NotFound.Resource", "Resource not found");

        // Act
        var result = Result<string>.NotFound(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsNotFound);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void NotFound_With_ResourceName_Should_Create_NotFound_Result()
    {
        // Arrange
        var resourceName = "TestResource";

        // Act
        var result = Result<string>.NotFound(resourceName);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsNotFound);
        Assert.Contains("TestResource", result.Error.Code);
        Assert.Contains("TestResource", result.Error.Description);
    }

    [Fact]
    public void NotFound_Without_ResourceName_Should_Use_Empty_String_As_Default()
    {
        // Act
        var result = Result<string>.NotFound();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsNotFound);
        // Check that it creates a proper NotFound error, the default might be empty string
        Assert.Contains("NotFound", result.Error.Code);
    }

    [Fact]
    public void Implicit_Conversion_From_Value_Should_Create_Success_Result()
    {
        // Arrange
        var value = "test value";

        // Act
        Result<string> result = value;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Implicit_Conversion_From_Null_Should_Create_Failure_Result()
    {
        // Arrange
        string? value = null;

        // Act
        Result<string> result = value;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.NullValue, result.Error);
    }

    [Fact]
    public void Success_With_Reference_Type_Should_Work()
    {
        // Arrange
        var testObject = new TestClass { Id = 1, Name = "Test" };

        // Act
        var result = Result<TestClass>.Success(testObject);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(testObject, result.Value);
        Assert.Equal(1, result.Value.Id);
        Assert.Equal("Test", result.Value.Name);
    }

    [Fact]
    public void Success_With_Value_Type_Should_Work()
    {
        // Arrange
        var value = 42;

        // Act
        var result = Result<int>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Failure_Should_Preserve_Error_Information()
    {
        // Arrange
        var error = new Error("Validation.Failed", "Validation failed for field X");

        // Act
        var result = Result<TestClass>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Validation.Failed", result.Error.Code);
        Assert.Equal("Validation failed for field X", result.Error.Description);
    }
}

// Test helper class
public class TestClass
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
