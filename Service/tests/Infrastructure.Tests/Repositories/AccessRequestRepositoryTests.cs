using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class AccessRequestRepositoryTests : TestBase
{
    private readonly AccessRequestRepository _repository;

    public AccessRequestRepositoryTests()
    {
        _repository = new AccessRequestRepository(Context);
    }

    [Fact]
    public async Task GetByRequestCodeAsync_WhenRequestExists_ShouldReturnAccessRequest()
    {
        // Arrange
        var requestCode = Guid.NewGuid();
        var accessRequest = new AccessRequest
        {
            RequestCode = requestCode,
            UserName = "testuser@example.com",
            JobNumber = 12345,
            CycleNumber = 1,
            ActivityCode = "ACT001",
            ApplicationName = "TestApp",
            Workstation = "WS001",
            UtcCreatedAt = DateTime.UtcNow
        };

        Context.AccessRequests.Add(accessRequest);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repository.GetByRequestCodeAsync(requestCode, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.RequestCode.Should().Be(requestCode);
        result.UserName.Should().Be("testuser@example.com");
        result.JobNumber.Should().Be(12345);
        result.ActivityCode.Should().Be("ACT001");
    }

    [Fact]
    public async Task GetByRequestCodeAsync_WhenRequestDoesNotExist_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByRequestCodeAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByJobNumberAsync_WhenRequestsExist_ShouldReturnRequestsOrderedByCreatedAtDesc()
    {
        // Arrange
        var jobNumber = 54321;
        var baseTime = DateTime.UtcNow;

        var accessRequests = new List<AccessRequest>
        {
            new()
            {
                RequestCode = Guid.NewGuid(),
                UserName = "user1@example.com",
                JobNumber = jobNumber,
                CycleNumber = 1,
                UtcCreatedAt = baseTime.AddMinutes(-10) // Oldest
            },
            new()
            {
                RequestCode = Guid.NewGuid(),
                UserName = "user2@example.com",
                JobNumber = jobNumber,
                CycleNumber = 2,
                UtcCreatedAt = baseTime // Newest
            },
            new()
            {
                RequestCode = Guid.NewGuid(),
                UserName = "user3@example.com",
                JobNumber = jobNumber,
                CycleNumber = 1,
                UtcCreatedAt = baseTime.AddMinutes(-5) // Middle
            },
            new()
            {
                RequestCode = Guid.NewGuid(),
                UserName = "user4@example.com",
                JobNumber = 99999, // Different job number - should not be returned
                CycleNumber = 1,
                UtcCreatedAt = baseTime
            }
        };

        Context.AccessRequests.AddRange(accessRequests);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var results = await _repository.GetByJobNumberAsync(jobNumber, TestContext.Current.CancellationToken);

        // Assert
        var resultList = results.ToList();
        resultList.Should().HaveCount(3);

        // Should be ordered by UtcCreatedAt descending
        resultList[0].UserName.Should().Be("user2@example.com"); // Newest
        resultList[1].UserName.Should().Be("user3@example.com"); // Middle
        resultList[2].UserName.Should().Be("user1@example.com"); // Oldest
    }

    [Fact]
    public async Task GetByJobNumberAsync_WhenNoRequestsExist_ShouldReturnEmptyCollection()
    {
        // Act
        var results = await _repository.GetByJobNumberAsync(99999, TestContext.Current.CancellationToken);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByUserNameAsync_WhenRequestsExist_ShouldReturnRequestsOrderedByCreatedAtDesc()
    {
        // Arrange
        var userName = "testuser@example.com";
        var baseTime = DateTime.UtcNow;

        var accessRequests = new List<AccessRequest>
        {
            new()
            {
                RequestCode = Guid.NewGuid(),
                UserName = userName,
                JobNumber = 111,
                CycleNumber = 1,
                UtcCreatedAt = baseTime.AddHours(-2) // Oldest
            },
            new()
            {
                RequestCode = Guid.NewGuid(),
                UserName = userName,
                JobNumber = 222,
                CycleNumber = 1,
                UtcCreatedAt = baseTime // Newest
            },
            new()
            {
                RequestCode = Guid.NewGuid(),
                UserName = userName,
                JobNumber = 333,
                CycleNumber = 1,
                UtcCreatedAt = baseTime.AddHours(-1) // Middle
            },
            new()
            {
                RequestCode = Guid.NewGuid(),
                UserName = "differentuser@example.com", // Different user - should not be returned
                JobNumber = 444,
                CycleNumber = 1,
                UtcCreatedAt = baseTime
            }
        };

        Context.AccessRequests.AddRange(accessRequests);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var results = await _repository.GetByUserNameAsync(userName, TestContext.Current.CancellationToken);

        // Assert
        var resultList = results.ToList();
        resultList.Should().HaveCount(3);

        // Should be ordered by UtcCreatedAt descending
        resultList[0].JobNumber.Should().Be(222); // Newest
        resultList[1].JobNumber.Should().Be(333); // Middle
        resultList[2].JobNumber.Should().Be(111); // Oldest
    }

    [Fact]
    public async Task GetByUserNameAsync_WhenNoRequestsExist_ShouldReturnEmptyCollection()
    {
        // Act
        var results = await _repository.GetByUserNameAsync("nonexistent@example.com", TestContext.Current.CancellationToken);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public async Task ExistsByRequestCodeAsync_WhenRequestExists_ShouldReturnTrue()
    {
        // Arrange
        var requestCode = Guid.NewGuid();
        var accessRequest = new AccessRequest
        {
            RequestCode = requestCode,
            UserName = "testuser@example.com",
            JobNumber = 12345,
            CycleNumber = 1,
            UtcCreatedAt = DateTime.UtcNow
        };

        Context.AccessRequests.Add(accessRequest);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repository.ExistsByRequestCodeAsync(requestCode, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByRequestCodeAsync_WhenRequestDoesNotExist_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.ExistsByRequestCodeAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task AddAsync_ShouldAddAccessRequestSuccessfully()
    {
        // Arrange
        var requestCode = Guid.NewGuid();
        var accessRequest = new AccessRequest
        {
            RequestCode = requestCode,
            UserName = "newuser@example.com",
            JobNumber = 67890,
            CycleNumber = 1,
            ActivityCode = "NEW001",
            ApplicationName = "NewApp",
            Workstation = "NEW-WS",
            UtcCreatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _repository.AddAsync(accessRequest, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.RequestCode.Should().Be(requestCode);

        // Verify using specific method
        var retrievedRequest = await _repository.GetByRequestCodeAsync(requestCode, TestContext.Current.CancellationToken);
        retrievedRequest.Should().NotBeNull();
        retrievedRequest!.UserName.Should().Be("newuser@example.com");
        retrievedRequest.JobNumber.Should().Be(67890);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAccessRequestSuccessfully()
    {
        // Arrange
        var requestCode = Guid.NewGuid();
        var accessRequest = new AccessRequest
        {
            RequestCode = requestCode,
            UserName = "original@example.com",
            JobNumber = 11111,
            CycleNumber = 1,
            ActivityCode = "ORIG001",
            UtcCreatedAt = DateTime.UtcNow
        };

        Context.AccessRequests.Add(accessRequest);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Detach to simulate fresh context
        Context.Entry(accessRequest).State = EntityState.Detached;

        // Modify request
        accessRequest.UserName = "updated@example.com";
        accessRequest.JobNumber = 22222;
        accessRequest.ActivityCode = "UPD001";

        // Act
        var result = await _repository.UpdateAsync(accessRequest, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.UserName.Should().Be("updated@example.com");
        result.JobNumber.Should().Be(22222);
        result.ActivityCode.Should().Be("UPD001");

        // Verify using specific method
        var updatedRequest = await _repository.GetByRequestCodeAsync(requestCode, TestContext.Current.CancellationToken);
        updatedRequest.Should().NotBeNull();
        updatedRequest!.UserName.Should().Be("updated@example.com");
        updatedRequest.JobNumber.Should().Be(22222);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteAccessRequestSuccessfully()
    {
        // Arrange
        var requestCode = Guid.NewGuid();
        var accessRequest = new AccessRequest
        {
            RequestCode = requestCode,
            UserName = "deleteme@example.com",
            JobNumber = 99999,
            CycleNumber = 1,
            UtcCreatedAt = DateTime.UtcNow
        };

        Context.AccessRequests.Add(accessRequest);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var requestId = accessRequest.Id;

        // Clear tracking to avoid conflicts
        ClearContext();

        // Act
        var result = await _repository.DeleteAsync(requestId, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(1);

        // Verify deletion
        var deletedRequest = await _repository.GetByRequestCodeAsync(requestCode, TestContext.Current.CancellationToken);
        deletedRequest.Should().BeNull();

        var existsResult = await _repository.ExistsByRequestCodeAsync(requestCode, TestContext.Current.CancellationToken);
        existsResult.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAccessRequests()
    {
        // Arrange
        var accessRequests = new List<AccessRequest>
        {
            new()
            {
                RequestCode = Guid.NewGuid(),
                UserName = "user1@example.com",
                JobNumber = 1001,
                CycleNumber = 1,
                UtcCreatedAt = DateTime.UtcNow
            },
            new()
            {
                RequestCode = Guid.NewGuid(),
                UserName = "user2@example.com",
                JobNumber = 1002,
                CycleNumber = 1,
                UtcCreatedAt = DateTime.UtcNow
            }
        };

        Context.AccessRequests.AddRange(accessRequests);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var results = await _repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        results.Should().HaveCount(2);
        results.Should().Contain(r => r.UserName == "user1@example.com");
        results.Should().Contain(r => r.UserName == "user2@example.com");
    }
}
