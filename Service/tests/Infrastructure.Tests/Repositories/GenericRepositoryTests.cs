using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class GenericRepositoryTests : TestBase
{
    private readonly GenericRepository<Activity> _repository;

    public GenericRepositoryTests()
    {
        _repository = new GenericRepository<Activity>(Context);
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityExists_ShouldReturnEntity()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "TEST001",
            ActivityName = "Test Activity",
            Description = "Test Description",
            IsActive = true
        };

        Context.Activities.Add(activity);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repository.GetByIdAsync(activity.Id, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(activity.Id);
        result.ActivityCode.Should().Be("TEST001");
        result.ActivityName.Should().Be("Test Activity");
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityDoesNotExist_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_WhenEntitiesExist_ShouldReturnAllEntities()
    {
        // Arrange
        var activities = new List<Activity>
        {
            new() { ActivityCode = "TEST001", ActivityName = "Activity 1", IsActive = true },
            new() { ActivityCode = "TEST002", ActivityName = "Activity 2", IsActive = true },
            new() { ActivityCode = "TEST003", ActivityName = "Activity 3", IsActive = false }
        };

        Context.Activities.AddRange(activities);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var results = await _repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        results.Should().HaveCount(3);
        results.Should().Contain(a => a.ActivityCode == "TEST001");
        results.Should().Contain(a => a.ActivityCode == "TEST002");
        results.Should().Contain(a => a.ActivityCode == "TEST003");
    }

    [Fact]
    public async Task GetAllAsync_WhenNoEntitiesExist_ShouldReturnEmptyCollection()
    {
        // Act
        var results = await _repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntityAndReturnIt()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "NEW001",
            ActivityName = "New Activity",
            Description = "New Description",
            IsActive = true
        };

        // Act
        var result = await _repository.AddAsync(activity, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.ActivityCode.Should().Be("NEW001");

        // Verify it was actually saved to the database
        var savedActivity = await Context.Activities.FindAsync(new object?[] { result.Id }, TestContext.Current.CancellationToken);
        savedActivity.Should().NotBeNull();
        savedActivity!.ActivityCode.Should().Be("NEW001");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntityAndReturnIt()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "UPDATE001",
            ActivityName = "Original Name",
            Description = "Original Description",
            IsActive = true
        };

        Context.Activities.Add(activity);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Detach the entity to simulate a fresh context
        Context.Entry(activity).State = EntityState.Detached;

        // Modify the activity
        activity.ActivityName = "Updated Name";
        activity.Description = "Updated Description";
        activity.IsActive = false;

        // Act
        var result = await _repository.UpdateAsync(activity, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.ActivityName.Should().Be("Updated Name");
        result.Description.Should().Be("Updated Description");
        result.IsActive.Should().BeFalse();

        // Verify it was actually updated in the database
        var updatedActivity = await Context.Activities.FindAsync(new object?[] { activity.Id }, TestContext.Current.CancellationToken);
        updatedActivity.Should().NotBeNull();
        updatedActivity!.ActivityName.Should().Be("Updated Name");
        updatedActivity.Description.Should().Be("Updated Description");
        updatedActivity.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityExists_ShouldDeleteEntityAndReturnRowsAffected()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "DELETE001",
            ActivityName = "To Delete",
            IsActive = true
        };

        Context.Activities.Add(activity);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var activityId = activity.Id;

        // Clear tracking to avoid conflicts
        ClearContext();

        // Act
        var result = await _repository.DeleteAsync(activityId, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(1); // One row affected

        // Verify it was actually deleted from the database
        var deletedActivity = await Context.Activities.FindAsync(new object?[] { activityId }, TestContext.Current.CancellationToken);
        deletedActivity.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityDoesNotExist_ShouldReturnZero()
    {
        // Act
        var result = await _repository.DeleteAsync(999, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ExistsAsync_WhenEntityExists_ShouldReturnTrue()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "EXISTS001",
            ActivityName = "Exists Test",
            IsActive = true
        };

        Context.Activities.Add(activity);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repository.ExistsAsync(activity.Id, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WhenEntityDoesNotExist_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.ExistsAsync(999, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
    }
}
