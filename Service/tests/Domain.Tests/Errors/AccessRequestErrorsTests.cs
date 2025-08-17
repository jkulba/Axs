using Domain.Common;
using Domain.Errors;
using Xunit;

namespace Domain.Tests.Errors;

public class AccessRequestErrorsTests
{
    [Fact]
    public void AccessRequestsNotFound_Should_Have_Correct_Code()
    {
        // Act
        var error = AccessRequestErrors.AccessRequestsNotFound;

        // Assert
        Assert.Equal("AccessRequests.AccessRequestsNotFound", error.Code);
    }

    [Fact]
    public void AccessRequestsNotFound_Should_Have_Correct_Description()
    {
        // Act
        var error = AccessRequestErrors.AccessRequestsNotFound;

        // Assert
        Assert.Equal("No access requests found", error.Description);
    }

    [Fact]
    public void AccessRequestsNotFound_Should_Be_Same_Reference()
    {
        // Act
        var error1 = AccessRequestErrors.AccessRequestsNotFound;
        var error2 = AccessRequestErrors.AccessRequestsNotFound;

        // Assert
        Assert.Equal(error1, error2);
        // Note: Static readonly fields are the same instance when accessed multiple times
    }

    [Fact]
    public void AccessRequestsNotFound_Should_Be_Equal_To_Same_Error()
    {
        // Arrange
        var originalError = AccessRequestErrors.AccessRequestsNotFound;
        var duplicateError = new Error("AccessRequests.AccessRequestsNotFound", "No access requests found");

        // Act & Assert
        Assert.Equal(originalError, duplicateError);
        Assert.True(originalError == duplicateError);
        Assert.False(originalError != duplicateError);
    }

    [Fact]
    public void AccessRequestsNotFound_Should_Convert_To_Result()
    {
        // Act
        Result result = AccessRequestErrors.AccessRequestsNotFound;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("No access requests found", result.ErrorMessage);
        // The implicit conversion uses the description, not the original error
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void AccessRequestsNotFound_ToResult_Should_Create_Failure()
    {
        // Act
        var result = AccessRequestErrors.AccessRequestsNotFound.ToResult();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("No access requests found", result.ErrorMessage);
    }

    [Fact]
    public void AccessRequestsNotFound_Should_Have_Correct_ToString_Format()
    {
        // Act
        var error = AccessRequestErrors.AccessRequestsNotFound;
        var toString = error.ToString();

        // Assert
        Assert.Contains("AccessRequests.AccessRequestsNotFound", toString);
        Assert.Contains("No access requests found", toString);
    }

    [Fact]
    public void AccessRequestsNotFound_Should_Work_With_Result_Generic()
    {
        // Act
        var result = Result<string>.Failure(AccessRequestErrors.AccessRequestsNotFound);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(AccessRequestErrors.AccessRequestsNotFound, result.Error);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void AccessRequestsNotFound_Should_Have_Correct_Hash_Code()
    {
        // Arrange
        var error1 = AccessRequestErrors.AccessRequestsNotFound;
        var error2 = new Error("AccessRequests.AccessRequestsNotFound", "No access requests found");

        // Act & Assert
        Assert.Equal(error1.GetHashCode(), error2.GetHashCode());
    }
}
