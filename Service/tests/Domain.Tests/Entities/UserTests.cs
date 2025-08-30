using Domain.Entities;
using Xunit;

namespace Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void User_Should_Have_Default_Constructor()
    {
        // Act
        var user = new User();

        // Assert
        Assert.NotNull(user);
        Assert.Equal(0, user.Id);
        // Note: These properties are marked as null! but will be null by default
        Assert.Null(user.GraphId);
        Assert.Null(user.UserId);
        Assert.Null(user.DisplayName);
        Assert.Null(user.PrincipalName);
        Assert.True(user.IsEnabled); // Default value is true
        Assert.Equal(DateTime.MinValue, user.LastUpdated);
        Assert.Equal(DateTime.MinValue, user.UtcCreatedAt);
        Assert.Null(user.CreatedByNum);
        Assert.Null(user.UtcUpdatedAt);
        Assert.Null(user.UpdatedByNum);
        Assert.NotNull(user.Authorizations);
        Assert.Empty(user.Authorizations);
        Assert.NotNull(user.UserGroupMemberships);
        Assert.Empty(user.UserGroupMemberships);
    }

    [Fact]
    public void User_Should_Allow_Setting_All_Properties()
    {
        // Arrange
        var id = 123;
        var graphId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890";
        var userId = "john.doe";
        var displayName = "John Doe";
        var principalName = "john.doe@company.com";
        var isEnabled = false;
        var lastUpdated = DateTime.UtcNow.AddDays(-1);
        var utcCreatedAt = DateTime.UtcNow.AddDays(-2);
        var createdByNum = "12345";
        var utcUpdatedAt = DateTime.UtcNow;
        var updatedByNum = "67890";

        // Act
        var user = new User
        {
            Id = id,
            GraphId = graphId,
            UserId = userId,
            DisplayName = displayName,
            PrincipalName = principalName,
            IsEnabled = isEnabled,
            LastUpdated = lastUpdated,
            UtcCreatedAt = utcCreatedAt,
            CreatedByNum = createdByNum,
            UtcUpdatedAt = utcUpdatedAt,
            UpdatedByNum = updatedByNum
        };

        // Assert
        Assert.Equal(id, user.Id);
        Assert.Equal(graphId, user.GraphId);
        Assert.Equal(userId, user.UserId);
        Assert.Equal(displayName, user.DisplayName);
        Assert.Equal(principalName, user.PrincipalName);
        Assert.Equal(isEnabled, user.IsEnabled);
        Assert.Equal(lastUpdated, user.LastUpdated);
        Assert.Equal(utcCreatedAt, user.UtcCreatedAt);
        Assert.Equal(createdByNum, user.CreatedByNum);
        Assert.Equal(utcUpdatedAt, user.UtcUpdatedAt);
        Assert.Equal(updatedByNum, user.UpdatedByNum);
    }

    [Fact]
    public void User_Should_Allow_Null_Optional_Properties()
    {
        // Act
        var user = new User
        {
            Id = 1,
            GraphId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
            UserId = "test.user",
            DisplayName = "Test User",
            PrincipalName = "test.user@company.com",
            IsEnabled = true,
            LastUpdated = DateTime.UtcNow,
            UtcCreatedAt = DateTime.UtcNow,
            CreatedByNum = null,
            UtcUpdatedAt = null,
            UpdatedByNum = null
        };

        // Assert
        Assert.Null(user.CreatedByNum);
        Assert.Null(user.UtcUpdatedAt);
        Assert.Null(user.UpdatedByNum);
    }

    [Fact]
    public void User_Should_Have_IsEnabled_Default_True()
    {
        // Act
        var user = new User();

        // Assert
        Assert.True(user.IsEnabled);
    }

    [Fact]
    public void User_Should_Initialize_Navigation_Properties_As_Empty_Collections()
    {
        // Act
        var user = new User();

        // Assert
        Assert.NotNull(user.Authorizations);
        Assert.IsType<List<Authorization>>(user.Authorizations);
        Assert.Empty(user.Authorizations);
        
        Assert.NotNull(user.UserGroupMemberships);
        Assert.IsType<List<UserGroupMember>>(user.UserGroupMemberships);
        Assert.Empty(user.UserGroupMemberships);
    }

    [Fact]
    public void User_Should_Allow_Adding_Authorizations()
    {
        // Arrange
        var user = new User();
        var authorization = new Authorization
        {
            AuthorizationId = 1,
            UserId = "user123",
            ActivityId = 1,
            JobNumber = 12345,
            UtcCreatedAt = DateTime.UtcNow
        };

        // Act
        user.Authorizations.Add(authorization);

        // Assert
        Assert.Single(user.Authorizations);
        Assert.Contains(authorization, user.Authorizations);
    }

    [Fact]
    public void User_Should_Allow_Adding_UserGroupMemberships()
    {
        // Arrange
        var user = new User();
        var userGroupMember = new UserGroupMember
        {
            UserId = "user123",
            GroupId = 1
        };

        // Act
        user.UserGroupMemberships.Add(userGroupMember);

        // Assert
        Assert.Single(user.UserGroupMemberships);
        Assert.Contains(userGroupMember, user.UserGroupMemberships);
    }

    [Fact]
    public void User_Should_Support_Multiple_Authorizations()
    {
        // Arrange
        var user = new User();
        var authorization1 = new Authorization
        {
            AuthorizationId = 1,
            UserId = "user123",
            ActivityId = 1,
            JobNumber = 12345,
            UtcCreatedAt = DateTime.UtcNow
        };
        var authorization2 = new Authorization
        {
            AuthorizationId = 2,
            UserId = "user123",
            ActivityId = 2,
            JobNumber = 12346,
            UtcCreatedAt = DateTime.UtcNow
        };

        // Act
        user.Authorizations.Add(authorization1);
        user.Authorizations.Add(authorization2);

        // Assert
        Assert.Equal(2, user.Authorizations.Count);
        Assert.Contains(authorization1, user.Authorizations);
        Assert.Contains(authorization2, user.Authorizations);
    }

    [Fact]
    public void User_Should_Support_Multiple_UserGroupMemberships()
    {
        // Arrange
        var user = new User();
        var membership1 = new UserGroupMember
        {
            UserId = "user123",
            GroupId = 1
        };
        var membership2 = new UserGroupMember
        {
            UserId = "user123",
            GroupId = 2
        };

        // Act
        user.UserGroupMemberships.Add(membership1);
        user.UserGroupMemberships.Add(membership2);

        // Assert
        Assert.Equal(2, user.UserGroupMemberships.Count);
        Assert.Contains(membership1, user.UserGroupMemberships);
        Assert.Contains(membership2, user.UserGroupMemberships);
    }

    [Theory]
    [InlineData("a1b2c3d4-e5f6-7890-abcd-ef1234567890")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF")]
    public void User_Should_Accept_Valid_GraphId_Formats(string graphId)
    {
        // Act
        var user = new User { GraphId = graphId };

        // Assert
        Assert.Equal(graphId, user.GraphId);
    }

    [Theory]
    [InlineData("john.doe")]
    [InlineData("user_123")]
    [InlineData("test-user")]
    [InlineData("simple")]
    [InlineData("user.with.dots")]
    [InlineData("user_with_underscores")]
    [InlineData("user-with-dashes")]
    [InlineData("user123")]
    public void User_Should_Accept_Valid_UserId_Formats(string userId)
    {
        // Act
        var user = new User { UserId = userId };

        // Assert
        Assert.Equal(userId, user.UserId);
    }

    [Theory]
    [InlineData("john.doe@company.com")]
    [InlineData("user@domain.org")]
    [InlineData("test.user@example.net")]
    [InlineData("admin@organization.gov")]
    public void User_Should_Accept_Valid_PrincipalName_Formats(string principalName)
    {
        // Act
        var user = new User { PrincipalName = principalName };

        // Assert
        Assert.Equal(principalName, user.PrincipalName);
    }

    [Theory]
    [InlineData("John Doe")]
    [InlineData("Jane Smith")]
    [InlineData("Test User")]
    [InlineData("Administrator")]
    [InlineData("System Account")]
    public void User_Should_Accept_Valid_DisplayName_Formats(string displayName)
    {
        // Act
        var user = new User { DisplayName = displayName };

        // Assert
        Assert.Equal(displayName, user.DisplayName);
    }

    [Fact]
    public void User_Should_Support_Audit_Fields_For_Creation()
    {
        // Arrange
        var createdAt = DateTime.UtcNow;
        var createdBy = "12345";

        // Act
        var user = new User
        {
            UtcCreatedAt = createdAt,
            CreatedByNum = createdBy,
            LastUpdated = createdAt
        };

        // Assert
        Assert.Equal(createdAt, user.UtcCreatedAt);
        Assert.Equal(createdBy, user.CreatedByNum);
        Assert.Equal(createdAt, user.LastUpdated);
        Assert.Null(user.UtcUpdatedAt);
        Assert.Null(user.UpdatedByNum);
    }

    [Fact]
    public void User_Should_Support_Audit_Fields_For_Updates()
    {
        // Arrange
        var createdAt = DateTime.UtcNow.AddDays(-1);
        var updatedAt = DateTime.UtcNow;
        var createdBy = "12345";
        var updatedBy = "67890";

        // Act
        var user = new User
        {
            UtcCreatedAt = createdAt,
            CreatedByNum = createdBy,
            UtcUpdatedAt = updatedAt,
            UpdatedByNum = updatedBy,
            LastUpdated = updatedAt
        };

        // Assert
        Assert.Equal(createdAt, user.UtcCreatedAt);
        Assert.Equal(createdBy, user.CreatedByNum);
        Assert.Equal(updatedAt, user.UtcUpdatedAt);
        Assert.Equal(updatedBy, user.UpdatedByNum);
        Assert.Equal(updatedAt, user.LastUpdated);
    }

    [Fact]
    public void User_Should_Allow_Removing_Authorizations()
    {
        // Arrange
        var user = new User();
        var authorization = new Authorization
        {
            AuthorizationId = 1,
            UserId = "user123",
            ActivityId = 1,
            JobNumber = 12345,
            UtcCreatedAt = DateTime.UtcNow
        };
        user.Authorizations.Add(authorization);

        // Act
        var removed = user.Authorizations.Remove(authorization);

        // Assert
        Assert.True(removed);
        Assert.Empty(user.Authorizations);
    }

    [Fact]
    public void User_Should_Allow_Removing_UserGroupMemberships()
    {
        // Arrange
        var user = new User();
        var membership = new UserGroupMember
        {
            UserId = "user123",
            GroupId = 1
        };
        user.UserGroupMemberships.Add(membership);

        // Act
        var removed = user.UserGroupMemberships.Remove(membership);

        // Assert
        Assert.True(removed);
        Assert.Empty(user.UserGroupMemberships);
    }

    [Fact]
    public void User_Should_Allow_Clearing_All_Authorizations()
    {
        // Arrange
        var user = new User();
        user.Authorizations.Add(new Authorization { AuthorizationId = 1, UserId = "user123", ActivityId = 1, JobNumber = 1, UtcCreatedAt = DateTime.UtcNow });
        user.Authorizations.Add(new Authorization { AuthorizationId = 2, UserId = "user123", ActivityId = 2, JobNumber = 2, UtcCreatedAt = DateTime.UtcNow });

        // Act
        user.Authorizations.Clear();

        // Assert
        Assert.Empty(user.Authorizations);
    }

    [Fact]
    public void User_Should_Allow_Clearing_All_UserGroupMemberships()
    {
        // Arrange
        var user = new User();
        user.UserGroupMemberships.Add(new UserGroupMember { UserId = "user123", GroupId = 1 });
        user.UserGroupMemberships.Add(new UserGroupMember { UserId = "user123", GroupId = 2 });

        // Act
        user.UserGroupMemberships.Clear();

        // Assert
        Assert.Empty(user.UserGroupMemberships);
    }
}
