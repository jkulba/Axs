using Domain.Entities;
using Xunit;

namespace Domain.Tests.Entities;

public class AccessRequestTests
{
    [Fact]
    public void AccessRequest_Should_Have_Default_Constructor()
    {
        // Act
        var accessRequest = new AccessRequest();

        // Assert
        Assert.NotNull(accessRequest);
        Assert.Equal(0, accessRequest.RequestId);
        Assert.Equal(Guid.Empty, accessRequest.RequestCode);
        Assert.Null(accessRequest.UserName); // UserName is actually null by default, not empty string
        Assert.Equal(0, accessRequest.JobNumber);
        Assert.Equal(0, accessRequest.CycleNumber);
        Assert.Null(accessRequest.ActivityCode);
        Assert.Null(accessRequest.ApplicationName);
        Assert.Null(accessRequest.Workstation);
        Assert.Null(accessRequest.UtcCreatedAt);
    }

    [Fact]
    public void AccessRequest_Should_Allow_Setting_All_Properties()
    {
        // Arrange
        var requestId = 123;
        var requestCode = Guid.NewGuid();
        var userName = "john.doe";
        var jobNumber = 12345;
        var cycleNumber = 1;
        var activityCode = "EDIT";
        var application = "TestApp";
        var workstation = "TESTMACHINE";
        var utcCreatedAt = DateTime.UtcNow;

        // Act
        var accessRequest = new AccessRequest
        {
            RequestId = requestId,
            RequestCode = requestCode,
            UserName = userName,
            JobNumber = jobNumber,
            CycleNumber = cycleNumber,
            ActivityCode = activityCode,
            ApplicationName = application,
            Workstation = workstation,
            UtcCreatedAt = utcCreatedAt
        };

        // Assert
        Assert.Equal(requestId, accessRequest.RequestId);
        Assert.Equal(requestCode, accessRequest.RequestCode);
        Assert.Equal(userName, accessRequest.UserName);
        Assert.Equal(jobNumber, accessRequest.JobNumber);
        Assert.Equal(cycleNumber, accessRequest.CycleNumber);
        Assert.Equal(activityCode, accessRequest.ActivityCode);
        Assert.Equal(application, accessRequest.ApplicationName);
        Assert.Equal(workstation, accessRequest.Workstation);
        Assert.Equal(utcCreatedAt, accessRequest.UtcCreatedAt);
    }

    [Fact]
    public void AccessRequest_Should_Allow_Null_Optional_Properties()
    {
        // Act
        var accessRequest = new AccessRequest
        {
            RequestId = 1,
            RequestCode = Guid.NewGuid(),
            UserName = "test.user",
            JobNumber = 123,
            CycleNumber = 1,
            ActivityCode = null,
            ApplicationName = null,
            Workstation = null,
            UtcCreatedAt = null
        };

        // Assert
        Assert.Null(accessRequest.ActivityCode);
        Assert.Null(accessRequest.ApplicationName);
        Assert.Null(accessRequest.Workstation);
        Assert.Null(accessRequest.UtcCreatedAt);
    }

    [Fact]
    public void AccessRequest_UserName_Can_Be_Null_By_Default()
    {
        // Act
        var accessRequest = new AccessRequest();

        // Assert
        Assert.Null(accessRequest.UserName); // Actually null by default
    }

    [Fact]
    public void AccessRequest_Should_Support_Object_Initialization()
    {
        // Arrange
        var expectedRequestCode = Guid.NewGuid();
        var expectedUserName = "jane.doe";
        var expectedJobNumber = 54321;

        // Act
        var accessRequest = new AccessRequest
        {
            RequestCode = expectedRequestCode,
            UserName = expectedUserName,
            JobNumber = expectedJobNumber
        };

        // Assert
        Assert.Equal(expectedRequestCode, accessRequest.RequestCode);
        Assert.Equal(expectedUserName, accessRequest.UserName);
        Assert.Equal(expectedJobNumber, accessRequest.JobNumber);
    }

    [Fact]
    public void AccessRequest_Should_Handle_DateTime_Precision()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var accessRequest = new AccessRequest();

        // Act
        accessRequest.UtcCreatedAt = now;

        // Assert
        Assert.Equal(now, accessRequest.UtcCreatedAt);
        Assert.Equal(DateTimeKind.Utc, accessRequest.UtcCreatedAt?.Kind);
    }

    [Fact]
    public void AccessRequest_Should_Handle_Large_Job_Numbers()
    {
        // Arrange
        var largeJobNumber = int.MaxValue;
        var accessRequest = new AccessRequest();

        // Act
        accessRequest.JobNumber = largeJobNumber;

        // Assert
        Assert.Equal(largeJobNumber, accessRequest.JobNumber);
    }

    [Fact]
    public void AccessRequest_Should_Handle_Long_Strings()
    {
        // Arrange
        var longUserName = new string('a', 1000);
        var longActivityCode = new string('b', 500);
        var longApplication = new string('c', 300);
        var accessRequest = new AccessRequest();

        // Act
        accessRequest.UserName = longUserName;
        accessRequest.ActivityCode = longActivityCode;
        accessRequest.ApplicationName = longApplication;

        // Assert
        Assert.Equal(longUserName, accessRequest.UserName);
        Assert.Equal(longActivityCode, accessRequest.ActivityCode);
        Assert.Equal(longApplication, accessRequest.ApplicationName);
    }

    [Fact]
    public void AccessRequest_Properties_Should_Be_Mutable()
    {
        // Arrange
        var accessRequest = new AccessRequest
        {
            UserName = "initial.user",
            JobNumber = 100
        };

        // Act
        accessRequest.UserName = "updated.user";
        accessRequest.JobNumber = 200;

        // Assert
        Assert.Equal("updated.user", accessRequest.UserName);
        Assert.Equal(200, accessRequest.JobNumber);
    }
}
