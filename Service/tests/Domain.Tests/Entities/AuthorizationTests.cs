using Domain.Entities;
using Xunit;

namespace Domain.Tests.Entities;

public class AuthorizationTests
{
    [Fact]
    public void Authorization_Should_Have_Default_Constructor()
    {
        // Act
        var authorization = new Authorization();

        // Assert
        Assert.NotNull(authorization);
        Assert.Equal(0, authorization.AuthorizationId);
        Assert.Equal(0, authorization.JobNumber);
        Assert.Null(authorization.UserId); // Actually null by default
        Assert.Equal(0, authorization.ActivityId);
        Assert.False(authorization.IsAuthorized);
        Assert.Equal(default(DateTime), authorization.UtcCreatedAt);
        Assert.Null(authorization.CreatedByNum);
        Assert.Null(authorization.UtcUpdatedAt);
        Assert.Null(authorization.UpdatedByNum);
        Assert.Null(authorization.Activity); // Actually null by default
    }

    [Fact]
    public void Authorization_Should_Allow_Setting_All_Properties()
    {
        // Arrange
        var authorizationId = 123;
        var jobNumber = 12345;
        var userId = "john.doe";
        var activityId = 5;
        var isAuthorized = true;
        var utcCreatedAt = DateTime.UtcNow;
        var createdByNum = "admin123";
        var utcUpdatedAt = DateTime.UtcNow.AddHours(1);
        var updatedByNum = "manager456";
        var activity = new Activity { ActivityId = 5, ActivityCode = "EDIT" };

        // Act
        var authorization = new Authorization
        {
            AuthorizationId = authorizationId,
            JobNumber = jobNumber,
            UserId = userId,
            ActivityId = activityId,
            IsAuthorized = isAuthorized,
            UtcCreatedAt = utcCreatedAt,
            CreatedByNum = createdByNum,
            UtcUpdatedAt = utcUpdatedAt,
            UpdatedByNum = updatedByNum,
            Activity = activity
        };

        // Assert
        Assert.Equal(authorizationId, authorization.AuthorizationId);
        Assert.Equal(jobNumber, authorization.JobNumber);
        Assert.Equal(userId, authorization.UserId);
        Assert.Equal(activityId, authorization.ActivityId);
        Assert.Equal(isAuthorized, authorization.IsAuthorized);
        Assert.Equal(utcCreatedAt, authorization.UtcCreatedAt);
        Assert.Equal(createdByNum, authorization.CreatedByNum);
        Assert.Equal(utcUpdatedAt, authorization.UtcUpdatedAt);
        Assert.Equal(updatedByNum, authorization.UpdatedByNum);
        Assert.Equal(activity, authorization.Activity);
    }

    [Fact]
    public void Authorization_Should_Support_Object_Initialization()
    {
        // Act
        var authorization = new Authorization
        {
            JobNumber = 54321,
            UserId = "jane.smith",
            ActivityId = 10,
            IsAuthorized = true
        };

        // Assert
        Assert.Equal(54321, authorization.JobNumber);
        Assert.Equal("jane.smith", authorization.UserId);
        Assert.Equal(10, authorization.ActivityId);
        Assert.True(authorization.IsAuthorized);
    }

    [Fact]
    public void Authorization_UserId_Is_Null_By_Default()
    {
        // Act
        var authorization = new Authorization();

        // Assert
        Assert.Null(authorization.UserId); // Actually null by default
    }

    [Fact]
    public void Authorization_Activity_Is_Null_By_Default()
    {
        // Act
        var authorization = new Authorization();

        // Assert
        Assert.Null(authorization.Activity); // Actually null by default
    }

    [Fact]
    public void Authorization_Optional_Properties_Can_Be_Null()
    {
        // Act
        var authorization = new Authorization
        {
            JobNumber = 123,
            UserId = "test.user",
            CreatedByNum = null,
            UtcUpdatedAt = null,
            UpdatedByNum = null
        };

        // Assert
        Assert.Null(authorization.CreatedByNum);
        Assert.Null(authorization.UtcUpdatedAt);
        Assert.Null(authorization.UpdatedByNum);
    }

    [Fact]
    public void Authorization_IsAuthorized_Should_Default_To_False()
    {
        // Act
        var authorization = new Authorization();

        // Assert
        Assert.False(authorization.IsAuthorized);
    }

    [Fact]
    public void Authorization_Should_Handle_DateTime_Precision()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var later = now.AddMinutes(30);
        var authorization = new Authorization();

        // Act
        authorization.UtcCreatedAt = now;
        authorization.UtcUpdatedAt = later;

        // Assert
        Assert.Equal(now, authorization.UtcCreatedAt);
        Assert.Equal(later, authorization.UtcUpdatedAt);
        Assert.Equal(DateTimeKind.Utc, authorization.UtcCreatedAt.Kind);
        Assert.Equal(DateTimeKind.Utc, authorization.UtcUpdatedAt?.Kind);
    }

    [Fact]
    public void Authorization_Should_Handle_Large_Job_Numbers()
    {
        // Arrange
        var largeJobNumber = int.MaxValue;
        var authorization = new Authorization();

        // Act
        authorization.JobNumber = largeJobNumber;

        // Assert
        Assert.Equal(largeJobNumber, authorization.JobNumber);
    }

    [Fact]
    public void Authorization_Should_Handle_Negative_Ids()
    {
        // Arrange
        var authorization = new Authorization();

        // Act
        authorization.AuthorizationId = -1;
        authorization.ActivityId = -5;

        // Assert
        Assert.Equal(-1, authorization.AuthorizationId);
        Assert.Equal(-5, authorization.ActivityId);
    }

    [Fact]
    public void Authorization_Properties_Should_Be_Mutable()
    {
        // Arrange
        var authorization = new Authorization
        {
            UserId = "initial.user",
            IsAuthorized = false,
            JobNumber = 100
        };

        // Act
        authorization.UserId = "updated.user";
        authorization.IsAuthorized = true;
        authorization.JobNumber = 200;

        // Assert
        Assert.Equal("updated.user", authorization.UserId);
        Assert.True(authorization.IsAuthorized);
        Assert.Equal(200, authorization.JobNumber);
    }

    [Fact]
    public void Authorization_Should_Handle_Long_User_Ids()
    {
        // Arrange
        var longUserId = new string('U', 1000);
        var authorization = new Authorization();

        // Act
        authorization.UserId = longUserId;

        // Assert
        Assert.Equal(longUserId, authorization.UserId);
    }

    [Fact]
    public void Authorization_Should_Handle_Activity_Navigation_Property()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityId = 10,
            ActivityCode = "APPROVE",
            ActivityName = "Approve Documents"
        };
        var authorization = new Authorization
        {
            ActivityId = 10,
            Activity = activity
        };

        // Act & Assert
        Assert.Equal(activity, authorization.Activity);
        Assert.Equal(10, authorization.ActivityId);
        Assert.Equal(authorization.ActivityId, authorization.Activity.ActivityId);
    }
}
