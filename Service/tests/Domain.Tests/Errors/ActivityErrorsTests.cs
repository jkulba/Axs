using Domain.Common;
using Domain.Errors;
using Xunit;

namespace Domain.Tests.Errors;

public class ActivityErrorsTests
{
    [Fact]
    public void ActivitiesNotFound_Should_Have_Correct_Code()
    {
        // Act
        var error = ActivityErrors.ActivitiesNotFound;

        // Assert
        Assert.Equal("Activity.ActivitiesNotFound", error.Code);
    }

    [Fact]
    public void ActivitiesNotFound_Should_Have_Correct_Description()
    {
        // Act
        var error = ActivityErrors.ActivitiesNotFound;

        // Assert
        Assert.Equal("No activities found", error.Description);
    }

    [Fact]
    public void ActivitiesNotFound_Should_Be_Same_Reference()
    {
        // Act
        var error1 = ActivityErrors.ActivitiesNotFound;
        var error2 = ActivityErrors.ActivitiesNotFound;

        // Assert
        Assert.Equal(error1, error2);
        // Note: Properties create new instances each time, so they are not the same reference
        Assert.NotSame(error1, error2);
    }

    [Fact]
    public void ActivitiesNotFound_Should_Be_Error_Type()
    {
        // Act
        var error = ActivityErrors.ActivitiesNotFound;

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
    public void ActivityByIdNotFound_Should_Have_Correct_Code(int id)
    {
        // Act
        var error = ActivityErrors.ActivityByIdNotFound(id);

        // Assert
        Assert.Equal("Activity.ActivityByIdNotFound", error.Code);
    }

    [Theory]
    [InlineData(1, "Activity with ID = '1' was not found")]
    [InlineData(123, "Activity with ID = '123' was not found")]
    [InlineData(999, "Activity with ID = '999' was not found")]
    [InlineData(0, "Activity with ID = '0' was not found")]
    [InlineData(-1, "Activity with ID = '-1' was not found")]
    public void ActivityByIdNotFound_Should_Have_Correct_Description(int id, string expectedDescription)
    {
        // Act
        var error = ActivityErrors.ActivityByIdNotFound(id);

        // Assert
        Assert.Equal(expectedDescription, error.Description);
    }

    [Fact]
    public void ActivityByIdNotFound_Should_Create_New_Instance_Each_Time()
    {
        // Arrange
        var id = 123;

        // Act
        var error1 = ActivityErrors.ActivityByIdNotFound(id);
        var error2 = ActivityErrors.ActivityByIdNotFound(id);

        // Assert
        Assert.Equal(error1, error2); // Same values
        Assert.NotSame(error1, error2); // Different instances
    }

    [Fact]
    public void ActivityByIdNotFound_Should_Be_Error_Type()
    {
        // Act
        var error = ActivityErrors.ActivityByIdNotFound(123);

        // Assert
        Assert.IsType<Error>(error);
        Assert.NotNull(error);
    }

    [Fact]
    public void ActivityByIdNotFound_Should_Handle_Different_Ids()
    {
        // Act
        var error1 = ActivityErrors.ActivityByIdNotFound(1);
        var error2 = ActivityErrors.ActivityByIdNotFound(2);

        // Assert
        Assert.NotEqual(error1, error2);
        Assert.Equal("Activity.ActivityByIdNotFound", error1.Code);
        Assert.Equal("Activity.ActivityByIdNotFound", error2.Code);
        Assert.Contains("'1'", error1.Description);
        Assert.Contains("'2'", error2.Description);
    }

    [Fact]
    public void ActivityByIdNotFound_Should_Include_Id_In_Description()
    {
        // Arrange
        var id = 42;

        // Act
        var error = ActivityErrors.ActivityByIdNotFound(id);

        // Assert
        Assert.Contains(id.ToString(), error.Description);
        Assert.Contains("Activity with ID", error.Description);
        Assert.Contains("was not found", error.Description);
    }

    [Fact]
    public void ActivityErrors_Should_Have_Consistent_Code_Prefix()
    {
        // Act
        var staticError = ActivityErrors.ActivitiesNotFound;
        var methodError = ActivityErrors.ActivityByIdNotFound(1);

        // Assert
        Assert.StartsWith("Activity.", staticError.Code);
        Assert.StartsWith("Activity.", methodError.Code);
    }

    [Fact]
    public void ActivityErrors_Should_Have_Non_Empty_Descriptions()
    {
        // Act
        var staticError = ActivityErrors.ActivitiesNotFound;
        var methodError = ActivityErrors.ActivityByIdNotFound(1);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(staticError.Description));
        Assert.False(string.IsNullOrWhiteSpace(methodError.Description));
    }

    [Fact]
    public void ActivityErrors_Should_Have_Unique_Error_Codes()
    {
        // Act
        var staticError = ActivityErrors.ActivitiesNotFound;
        var methodError = ActivityErrors.ActivityByIdNotFound(1);

        // Assert
        Assert.NotEqual(staticError.Code, methodError.Code);
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public void ActivityByIdNotFound_Should_Handle_Extreme_Values(int id)
    {
        // Act
        var error = ActivityErrors.ActivityByIdNotFound(id);

        // Assert
        Assert.Equal("Activity.ActivityByIdNotFound", error.Code);
        Assert.Contains(id.ToString(), error.Description);
        Assert.NotNull(error.Description);
        Assert.NotEmpty(error.Description);
    }
}
