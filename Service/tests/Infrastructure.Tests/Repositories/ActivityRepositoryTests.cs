using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class ActivityRepositoryTests : TestBase
{
    private readonly ActivityRepository _repository;

    public ActivityRepositoryTests()
    {
        _repository = new ActivityRepository(Context);
    }

    [Fact]
    public async Task GetByIdAsync_WhenActivityExists_ShouldReturnActivity()
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
        result.Description.Should().Be("Test Description");
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_WhenActivityDoesNotExist_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_WhenActivitiesExist_ShouldReturnAllActivities()
    {
        // Arrange
        var activities = new List<Activity>
        {
            new()
            {
                ActivityCode = "ACT001",
                ActivityName = "Activity One",
                Description = "First activity",
                IsActive = true
            },
            new()
            {
                ActivityCode = "ACT002",
                ActivityName = "Activity Two",
                Description = "Second activity",
                IsActive = false
            },
            new()
            {
                ActivityCode = "ACT003",
                ActivityName = "Activity Three",
                Description = "Third activity",
                IsActive = true
            }
        };

        Context.Activities.AddRange(activities);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var results = await _repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        results.Should().HaveCount(3);
        results.Should().Contain(a => a.ActivityCode == "ACT001");
        results.Should().Contain(a => a.ActivityCode == "ACT002");
        results.Should().Contain(a => a.ActivityCode == "ACT003");

        var activeActivities = results.Where(a => a.IsActive).ToList();
        activeActivities.Should().HaveCount(2);

        var inactiveActivities = results.Where(a => !a.IsActive).ToList();
        inactiveActivities.Should().HaveCount(1);
        inactiveActivities.First().ActivityCode.Should().Be("ACT002");
    }

    [Fact]
    public async Task GetAllAsync_WhenNoActivitiesExist_ShouldReturnEmptyCollection()
    {
        // Act
        var results = await _repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public async Task AddAsync_ShouldAddActivitySuccessfully()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "NEW001",
            ActivityName = "New Activity",
            Description = "Newly created activity",
            IsActive = true
        };

        // Act
        var result = await _repository.AddAsync(activity, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.ActivityCode.Should().Be("NEW001");
        result.ActivityName.Should().Be("New Activity");
        result.Description.Should().Be("Newly created activity");
        result.IsActive.Should().BeTrue();

        // Verify it was actually saved to the database
        var savedActivity = await Context.Activities.FindAsync(new object?[] { result.Id }, TestContext.Current.CancellationToken);
        savedActivity.Should().NotBeNull();
        savedActivity!.ActivityCode.Should().Be("NEW001");
    }

    [Fact]
    public async Task AddAsync_WithMinimalData_ShouldWork()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "MIN001",
            ActivityName = "Minimal Activity"
            // Description is optional, IsActive defaults to true
        };

        // Act
        var result = await _repository.AddAsync(activity, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.ActivityCode.Should().Be("MIN001");
        result.ActivityName.Should().Be("Minimal Activity");
        result.Description.Should().BeNull();
        result.IsActive.Should().BeTrue(); // Default value
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateActivitySuccessfully()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "UPD001",
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
    public async Task UpdateAsync_PartialUpdate_ShouldUpdateOnlyChangedFields()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "PART001",
            ActivityName = "Original Name",
            Description = "Original Description",
            IsActive = true
        };

        Context.Activities.Add(activity);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Detach the entity
        Context.Entry(activity).State = EntityState.Detached;

        // Only modify IsActive
        activity.IsActive = false;

        // Act
        var result = await _repository.UpdateAsync(activity, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.ActivityCode.Should().Be("PART001"); // Unchanged
        result.ActivityName.Should().Be("Original Name"); // Unchanged
        result.Description.Should().Be("Original Description"); // Unchanged
        result.IsActive.Should().BeFalse(); // Changed
    }

    [Fact]
    public async Task DeleteAsync_WhenActivityExists_ShouldDeleteActivityAndReturnRowsAffected()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "DEL001",
            ActivityName = "To Delete",
            Description = "Activity to be deleted",
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
    public async Task DeleteAsync_WhenActivityDoesNotExist_ShouldReturnZero()
    {
        // Act
        var result = await _repository.DeleteAsync(999, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task ExistsAsync_WhenActivityExists_ShouldReturnTrue()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityCode = "EXI001",
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
    public async Task ExistsAsync_WhenActivityDoesNotExist_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.ExistsAsync(999, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Repository_ShouldHandleMultipleOperationsInSequence()
    {
        // Arrange & Act & Assert - Integration test

        // 1. Add an activity
        var activity = new Activity
        {
            ActivityCode = "SEQ001",
            ActivityName = "Sequence Test",
            Description = "Testing sequence of operations",
            IsActive = true
        };

        var addedActivity = await _repository.AddAsync(activity, TestContext.Current.CancellationToken);
        addedActivity.Should().NotBeNull();
        addedActivity.Id.Should().BeGreaterThan(0);

        // 2. Verify it exists
        var exists = await _repository.ExistsAsync(addedActivity.Id, TestContext.Current.CancellationToken);
        exists.Should().BeTrue();

        // 3. Get by ID
        var retrievedActivity = await _repository.GetByIdAsync(addedActivity.Id, TestContext.Current.CancellationToken);
        retrievedActivity.Should().NotBeNull();
        retrievedActivity!.ActivityCode.Should().Be("SEQ001");

        // 4. Update it - Clear tracking first to avoid conflicts
        ClearContext();
        retrievedActivity.ActivityName = "Updated Sequence Test";
        var updatedActivity = await _repository.UpdateAsync(retrievedActivity, TestContext.Current.CancellationToken);
        updatedActivity.ActivityName.Should().Be("Updated Sequence Test");

        // 5. Verify update
        var reRetrievedActivity = await _repository.GetByIdAsync(addedActivity.Id, TestContext.Current.CancellationToken);
        reRetrievedActivity!.ActivityName.Should().Be("Updated Sequence Test");

        // 6. Delete it - Clear tracking first to avoid conflicts
        ClearContext();
        var deleteResult = await _repository.DeleteAsync(addedActivity.Id, TestContext.Current.CancellationToken);
        deleteResult.Should().Be(1);

        // 7. Verify deletion
        var finalExists = await _repository.ExistsAsync(addedActivity.Id, TestContext.Current.CancellationToken);
        finalExists.Should().BeFalse();

        var finalGet = await _repository.GetByIdAsync(addedActivity.Id, TestContext.Current.CancellationToken);
        finalGet.Should().BeNull();
    }
}
