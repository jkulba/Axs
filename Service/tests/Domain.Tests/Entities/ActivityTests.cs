using Domain.Entities;
using Xunit;

namespace Domain.Tests.Entities;

public class ActivityTests
{
    [Fact]
    public void Activity_Should_Have_Default_Constructor()
    {
        // Act
        var activity = new Activity();

        // Assert
        Assert.NotNull(activity);
        Assert.Equal(0, activity.Id);
        Assert.Null(activity.ActivityCode); // Actually null by default
        Assert.Null(activity.ActivityName); // Actually null by default
        Assert.Null(activity.Description);
        Assert.True(activity.IsActive); // Default should be true
        Assert.NotNull(activity.Authorizations);
        Assert.Empty(activity.Authorizations);
        Assert.NotNull(activity.GroupAuthorizations);
        Assert.Empty(activity.GroupAuthorizations);
    }

    [Fact]
    public void Activity_Should_Allow_Setting_All_Properties()
    {
        // Arrange
        var activityId = 123;
        var activityCode = "EDIT";
        var activityName = "Edit Documents";
        var description = "Allows editing of documents";
        var isActive = false;

        // Act
        var activity = new Activity
        {
            Id = activityId,
            ActivityCode = activityCode,
            ActivityName = activityName,
            Description = description,
            IsActive = isActive
        };

        // Assert
        Assert.Equal(activityId, activity.Id);
        Assert.Equal(activityCode, activity.ActivityCode);
        Assert.Equal(activityName, activity.ActivityName);
        Assert.Equal(description, activity.Description);
        Assert.Equal(isActive, activity.IsActive);
    }

    [Fact]
    public void Activity_Should_Support_Object_Initialization()
    {
        // Act
        var activity = new Activity
        {
            ActivityCode = "VIEW",
            ActivityName = "View Documents",
            Description = "Read-only access to documents"
        };

        // Assert
        Assert.Equal("VIEW", activity.ActivityCode);
        Assert.Equal("View Documents", activity.ActivityName);
        Assert.Equal("Read-only access to documents", activity.Description);
        Assert.True(activity.IsActive); // Should maintain default
    }

    [Fact]
    public void Activity_Required_Properties_Are_Null_By_Default()
    {
        // Act
        var activity = new Activity();

        // Assert
        Assert.Null(activity.ActivityCode);
        Assert.Null(activity.ActivityName);
    }

    [Fact]
    public void Activity_Description_Can_Be_Null()
    {
        // Act
        var activity = new Activity
        {
            ActivityCode = "TEST",
            ActivityName = "Test Activity",
            Description = null
        };

        // Assert
        Assert.Null(activity.Description);
    }

    [Fact]
    public void Activity_IsActive_Should_Default_To_True()
    {
        // Act
        var activity = new Activity();

        // Assert
        Assert.True(activity.IsActive);
    }

    [Fact]
    public void Activity_IsActive_Should_Be_Settable_To_False()
    {
        // Arrange
        var activity = new Activity();

        // Act
        activity.IsActive = false;

        // Assert
        Assert.False(activity.IsActive);
    }

    [Fact]
    public void Activity_Navigation_Properties_Should_Be_Initialized()
    {
        // Act
        var activity = new Activity();

        // Assert
        Assert.NotNull(activity.Authorizations);
        Assert.NotNull(activity.GroupAuthorizations);
        Assert.IsType<List<Authorization>>(activity.Authorizations);
        Assert.IsType<List<GroupAuthorization>>(activity.GroupAuthorizations);
    }

    [Fact]
    public void Activity_Should_Allow_Adding_Authorizations()
    {
        // Arrange
        var activity = new Activity();
        var authorization = new Authorization
        {
            AuthorizationId = 1,
            JobNumber = 12345,
            UserId = "test.user"
        };

        // Act
        activity.Authorizations.Add(authorization);

        // Assert
        Assert.Single(activity.Authorizations);
        Assert.Contains(authorization, activity.Authorizations);
    }

    [Fact]
    public void Activity_Should_Allow_Adding_GroupAuthorizations()
    {
        // Arrange
        var activity = new Activity();
        var groupAuthorization = new GroupAuthorization
        {
            AuthorizationId = 1,
            JobNumber = 12345
        };

        // Act
        activity.GroupAuthorizations.Add(groupAuthorization);

        // Assert
        Assert.Single(activity.GroupAuthorizations);
        Assert.Contains(groupAuthorization, activity.GroupAuthorizations);
    }

    [Fact]
    public void Activity_Properties_Should_Be_Mutable()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "INITIAL",
            ActivityName = "Initial Name",
            IsActive = true
        };

        // Act
        activity.ActivityCode = "UPDATED";
        activity.ActivityName = "Updated Name";
        activity.IsActive = false;

        // Assert
        Assert.Equal("UPDATED", activity.ActivityCode);
        Assert.Equal("Updated Name", activity.ActivityName);
        Assert.False(activity.IsActive);
    }

    [Fact]
    public void Activity_Should_Handle_Long_Strings()
    {
        // Arrange
        var longCode = new string('A', 1000);
        var longName = new string('B', 2000);
        var longDescription = new string('C', 5000);

        // Act
        var activity = new Activity
        {
            ActivityCode = longCode,
            ActivityName = longName,
            Description = longDescription
        };

        // Assert
        Assert.Equal(longCode, activity.ActivityCode);
        Assert.Equal(longName, activity.ActivityName);
        Assert.Equal(longDescription, activity.Description);
    }
}
