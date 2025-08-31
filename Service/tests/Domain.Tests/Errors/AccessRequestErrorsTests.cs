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

    #region AccessRequestByRequestCodeNotFound Tests

    [Fact]
    public void AccessRequestByRequestCodeNotFound_Should_Have_Correct_Code()
    {
        // Arrange
        var requestCode = Guid.NewGuid();

        // Act
        var error = AccessRequestErrors.AccessRequestByRequestCodeNotFound(requestCode);

        // Assert
        Assert.Equal("AccessRequests.AccessRequestByRequestCodeNotFound", error.Code);
    }

    [Fact]
    public void AccessRequestByRequestCodeNotFound_Should_Have_Correct_Description_With_RequestCode()
    {
        // Arrange
        var requestCode = Guid.NewGuid();

        // Act
        var error = AccessRequestErrors.AccessRequestByRequestCodeNotFound(requestCode);

        // Assert
        Assert.Equal($"Access request with Request Code = '{requestCode}' was not found", error.Description);
    }

    [Fact]
    public void AccessRequestByRequestCodeNotFound_Should_Include_RequestCode_In_Description()
    {
        // Arrange
        var requestCode = Guid.Parse("12345678-1234-1234-1234-123456789012");

        // Act
        var error = AccessRequestErrors.AccessRequestByRequestCodeNotFound(requestCode);

        // Assert
        Assert.Contains("12345678-1234-1234-1234-123456789012", error.Description);
    }

    [Fact]
    public void AccessRequestByRequestCodeNotFound_Should_Handle_Empty_Guid()
    {
        // Arrange
        var requestCode = Guid.Empty;

        // Act
        var error = AccessRequestErrors.AccessRequestByRequestCodeNotFound(requestCode);

        // Assert
        Assert.Equal($"Access request with Request Code = '{Guid.Empty}' was not found", error.Description);
    }

    #endregion

    #region AccessRequestByIdNotFound Tests

    [Fact]
    public void AccessRequestByIdNotFound_Should_Have_Correct_Code()
    {
        // Arrange
        var id = 123;

        // Act
        var error = AccessRequestErrors.AccessRequestByIdNotFound(id);

        // Assert
        Assert.Equal("AccessRequests.AccessRequestByIdNotFound", error.Code);
    }

    [Fact]
    public void AccessRequestByIdNotFound_Should_Have_Correct_Description_With_Id()
    {
        // Arrange
        var id = 456;

        // Act
        var error = AccessRequestErrors.AccessRequestByIdNotFound(id);

        // Assert
        Assert.Equal($"Access request with ID = '{id}' was not found", error.Description);
    }

    [Fact]
    public void AccessRequestByIdNotFound_Should_Handle_Negative_Id()
    {
        // Arrange
        var id = -1;

        // Act
        var error = AccessRequestErrors.AccessRequestByIdNotFound(id);

        // Assert
        Assert.Equal("Access request with ID = '-1' was not found", error.Description);
    }

    [Fact]
    public void AccessRequestByIdNotFound_Should_Handle_Zero_Id()
    {
        // Arrange
        var id = 0;

        // Act
        var error = AccessRequestErrors.AccessRequestByIdNotFound(id);

        // Assert
        Assert.Equal("Access request with ID = '0' was not found", error.Description);
    }

    #endregion

    #region AccessRequestByUserNameNotFound Tests

    [Fact]
    public void AccessRequestByUserNameNotFound_Should_Have_Correct_Code()
    {
        // Arrange
        var userName = "testuser";

        // Act
        var error = AccessRequestErrors.AccessRequestByUserNameNotFound(userName);

        // Assert
        Assert.Equal("AccessRequests.AccessRequestByUserNameNotFound", error.Code);
    }

    [Fact]
    public void AccessRequestByUserNameNotFound_Should_Have_Correct_Description_With_UserName()
    {
        // Arrange
        var userName = "john.doe";

        // Act
        var error = AccessRequestErrors.AccessRequestByUserNameNotFound(userName);

        // Assert
        Assert.Equal($"Access request for User Name = '{userName}' was not found", error.Description);
    }

    [Fact]
    public void AccessRequestByUserNameNotFound_Should_Handle_Empty_UserName()
    {
        // Arrange
        var userName = "";

        // Act
        var error = AccessRequestErrors.AccessRequestByUserNameNotFound(userName);

        // Assert
        Assert.Equal("Access request for User Name = '' was not found", error.Description);
    }

    [Fact]
    public void AccessRequestByUserNameNotFound_Should_Handle_Null_UserName()
    {
        // Arrange
        string? userName = null;

        // Act
        var error = AccessRequestErrors.AccessRequestByUserNameNotFound(userName!);

        // Assert
        Assert.Equal("Access request for User Name = '' was not found", error.Description);
    }

    [Fact]
    public void AccessRequestByUserNameNotFound_Should_Handle_Special_Characters()
    {
        // Arrange
        var userName = "user@domain.com";

        // Act
        var error = AccessRequestErrors.AccessRequestByUserNameNotFound(userName);

        // Assert
        Assert.Contains("user@domain.com", error.Description);
    }

    #endregion

    #region AccessRequestByJobNumberNotFound Tests

    [Fact]
    public void AccessRequestByJobNumberNotFound_Should_Have_Correct_Code()
    {
        // Arrange
        var jobNumber = 12345;

        // Act
        var error = AccessRequestErrors.AccessRequestByJobNumberNotFound(jobNumber);

        // Assert
        Assert.Equal("AccessRequests.AccessRequestByJobNumberNotFound", error.Code);
    }

    [Fact]
    public void AccessRequestByJobNumberNotFound_Should_Have_Correct_Description_With_JobNumber()
    {
        // Arrange
        var jobNumber = 67890;

        // Act
        var error = AccessRequestErrors.AccessRequestByJobNumberNotFound(jobNumber);

        // Assert
        Assert.Equal($"Access request for Job Number = '{jobNumber}' was not found", error.Description);
    }

    [Fact]
    public void AccessRequestByJobNumberNotFound_Should_Handle_Negative_JobNumber()
    {
        // Arrange
        var jobNumber = -999;

        // Act
        var error = AccessRequestErrors.AccessRequestByJobNumberNotFound(jobNumber);

        // Assert
        Assert.Equal("Access request for Job Number = '-999' was not found", error.Description);
    }

    [Fact]
    public void AccessRequestByJobNumberNotFound_Should_Handle_Zero_JobNumber()
    {
        // Arrange
        var jobNumber = 0;

        // Act
        var error = AccessRequestErrors.AccessRequestByJobNumberNotFound(jobNumber);

        // Assert
        Assert.Equal("Access request for Job Number = '0' was not found", error.Description);
    }

    #endregion

    #region Cross-Method Consistency Tests

    [Fact]
    public void All_AccessRequestErrors_Should_Have_Consistent_Code_Prefix()
    {
        // Arrange & Act
        var errors = new[]
        {
            AccessRequestErrors.AccessRequestsNotFound,
            AccessRequestErrors.AccessRequestByRequestCodeNotFound(Guid.NewGuid()),
            AccessRequestErrors.AccessRequestByIdNotFound(1),
            AccessRequestErrors.AccessRequestByUserNameNotFound("test"),
            AccessRequestErrors.AccessRequestByJobNumberNotFound(1)
        };

        // Assert
        foreach (var error in errors)
        {
            Assert.StartsWith("AccessRequests.", error.Code);
        }
    }

    [Fact]
    public void All_AccessRequestErrors_Should_Have_Non_Empty_Descriptions()
    {
        // Arrange & Act
        var errors = new[]
        {
            AccessRequestErrors.AccessRequestsNotFound,
            AccessRequestErrors.AccessRequestByRequestCodeNotFound(Guid.NewGuid()),
            AccessRequestErrors.AccessRequestByIdNotFound(1),
            AccessRequestErrors.AccessRequestByUserNameNotFound("test"),
            AccessRequestErrors.AccessRequestByJobNumberNotFound(1)
        };

        // Assert
        foreach (var error in errors)
        {
            Assert.False(string.IsNullOrWhiteSpace(error.Description));
        }
    }

    [Fact]
    public void All_Parameterized_AccessRequestErrors_Should_Generate_Different_Instances()
    {
        // Act
        var error1 = AccessRequestErrors.AccessRequestByIdNotFound(1);
        var error2 = AccessRequestErrors.AccessRequestByIdNotFound(2);

        // Assert
        Assert.NotEqual(error1.Description, error2.Description);
        Assert.Equal(error1.Code, error2.Code); // Same code, different description
    }

    #endregion
}
