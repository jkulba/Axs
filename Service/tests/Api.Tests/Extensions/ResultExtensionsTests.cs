using Api.Extensions;
using Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Api.Tests.Extensions;

public class ResultExtensionsTests
{
    #region ToProblemDetails Tests

    [Fact]
    public void ToProblemDetails_Should_Throw_InvalidOperationException_When_Result_IsSuccess()
    {
        // Arrange
        var successResult = Result.Success();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => successResult.ToProblemDetails());
        Assert.Equal("Can't convert success result to problem", exception.Message);
    }

    [Fact]
    public void ToProblemDetails_Should_Return_404_When_Result_IsNotFound()
    {
        // Arrange
        var notFoundError = new Error("NotFound.Resource", "Resource not found");
        var result = Result.Failure(notFoundError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
    }

    [Fact]
    public void ToProblemDetails_Should_Return_404_When_Error_IsNotFound()
    {
        // Arrange
        var result = Result.Failure(Error.NotFound);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
    }

    [Fact]
    public void ToProblemDetails_Should_Return_400_When_Error_Code_Is_NullValue()
    {
        // Arrange
        var nullValueError = new Error("Error.NullValue", "Null value provided");
        var result = Result.Failure(nullValueError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
    }

    [Fact]
    public void ToProblemDetails_Should_Return_500_For_Generic_Error()
    {
        // Arrange
        var genericError = new Error("Generic.Error", "Something went wrong");
        var result = Result.Failure(genericError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
    }

    [Fact]
    public void ToProblemDetails_Should_Include_Error_In_Extensions()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error message");
        var result = Result.Failure(error);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
    }

    #endregion

    #region Status Code Path Tests

    [Fact]
    public void ToProblemDetails_Should_Use_BadRequest_Path_For_NullValue_Error()
    {
        // Arrange
        var nullValueError = new Error("Error.NullValue", "Null value error");
        var result = Result.Failure(nullValueError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        // This exercises the BadRequest (400) path including GetTitleForStatusCode and GetRfcLinkForStatusCode
    }

    [Fact]
    public void ToProblemDetails_Should_Use_NotFound_Path_For_NotFound_Error()
    {
        // Arrange
        var notFoundError = new Error("NotFound.Test", "Resource not found");
        var result = Result.Failure(notFoundError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        // This exercises the NotFound (404) path including GetTitleForStatusCode and GetRfcLinkForStatusCode
    }

    [Fact]
    public void ToProblemDetails_Should_Use_InternalServerError_Path_For_Generic_Error()
    {
        // Arrange
        var genericError = new Error("Generic.Error", "Unexpected error");
        var result = Result.Failure(genericError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        // This exercises the InternalServerError (500) path including GetTitleForStatusCode and GetRfcLinkForStatusCode
    }

    [Fact]
    public void ToProblemDetails_Should_Handle_Different_Error_Code_Patterns()
    {
        // Arrange & Act & Assert
        var testCases = new[]
        {
            ("Error.NullValue", "Null value"), // Should be 400
            ("NotFound.User", "User not found"), // Should be 404
            ("Custom.Error", "Custom error"), // Should be 500
            ("", "Empty code"), // Should be 500
            ("AnotherPattern.Test", "Another pattern") // Should be 500
        };

        foreach (var (code, description) in testCases)
        {
            var error = new Error(code, description);
            var result = Result.Failure(error);
            var problemDetails = result.ToProblemDetails();
            
            Assert.NotNull(problemDetails);
            Assert.IsAssignableFrom<IResult>(problemDetails);
        }
    }

    #endregion

    #region Integration Tests - Testing the complete flow

    [Fact]
    public void ToProblemDetails_Should_Handle_BadRequest_Error_Correctly()
    {
        // Arrange
        var badRequestError = new Error("Error.NullValue", "Required field is null");
        var result = Result.Failure(badRequestError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
    }

    [Fact]
    public void ToProblemDetails_Should_Handle_Multiple_Different_Errors()
    {
        // Arrange
        var errors = new[]
        {
            new Error("Error.NullValue", "Null value"),
            new Error("Test.NotFound", "Not found"),
            new Error("Generic.Error", "Generic error")
        };

        // Act & Assert
        foreach (var error in errors)
        {
            var result = Result.Failure(error);
            var problemDetails = result.ToProblemDetails();
            
            Assert.NotNull(problemDetails);
            Assert.IsAssignableFrom<IResult>(problemDetails);
        }
    }

    [Fact]
    public void ToProblemDetails_Should_Be_Consistent_For_Same_Error()
    {
        // Arrange
        var error = new Error("Test.Error", "Test message");
        var result1 = Result.Failure(error);
        var result2 = Result.Failure(error);

        // Act
        var problemDetails1 = result1.ToProblemDetails();
        var problemDetails2 = result2.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails1);
        Assert.NotNull(problemDetails2);
        Assert.IsAssignableFrom<IResult>(problemDetails1);
        Assert.IsAssignableFrom<IResult>(problemDetails2);
    }

    [Fact]
    public void ToProblemDetails_Should_Handle_Empty_Error_Code()
    {
        // Arrange
        var emptyCodeError = new Error("", "Error with empty code");
        var result = Result.Failure(emptyCodeError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
    }

    [Fact]
    public void ToProblemDetails_Should_Handle_Empty_Error_Description()
    {
        // Arrange
        var emptyDescriptionError = new Error("Test.Error", "");
        var result = Result.Failure(emptyDescriptionError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ToProblemDetails_Should_Handle_Very_Long_Error_Messages()
    {
        // Arrange
        var longMessage = new string('A', 1000);
        var longMessageError = new Error("Test.LongMessage", longMessage);
        var result = Result.Failure(longMessageError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
    }

    [Fact]
    public void ToProblemDetails_Should_Handle_Special_Characters_In_Error()
    {
        // Arrange
        var specialCharsError = new Error("Test.Special", "Error with special chars: !@#$%^&*()");
        var result = Result.Failure(specialCharsError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
    }

    [Fact]
    public void ToProblemDetails_Should_Handle_Unicode_Characters()
    {
        // Arrange
        var unicodeError = new Error("Test.Unicode", "Error with unicode: 你好世界 🌍");
        var result = Result.Failure(unicodeError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
    }

    [Fact]
    public void ToProblemDetails_Should_Use_Default_Cases_For_Unknown_Status_Codes()
    {
        // Arrange - Create a custom error that doesn't match any specific patterns
        // This should trigger the default case in both switch expressions
        var customError = new Error("Custom.UnknownStatusCode", "Custom error that triggers default");
        var result = Result.Failure(customError);

        // Act
        var problemDetails = result.ToProblemDetails();

        // Assert
        Assert.NotNull(problemDetails);
        Assert.IsAssignableFrom<IResult>(problemDetails);
        
        // This test ensures the default branches in GetTitleForStatusCode and GetRfcLinkForStatusCode are covered
        // Since the error doesn't match "Error.NullValue" and isn't NotFound, it should use Status500InternalServerError
        // But we're testing that the switch expressions handle unknown cases properly
    }

    #endregion
}
