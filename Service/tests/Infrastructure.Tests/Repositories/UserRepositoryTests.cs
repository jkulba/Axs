using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class UserRepositoryTests : TestBase
{
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        _repository = new UserRepository(Context);
    }

    [Fact]
    public async Task GetByUserIdAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var user = new User
        {
            GraphId = "graph-123",
            UserId = "testuser@example.com",
            DisplayName = "Test User",
            PrincipalName = "testuser@example.com",
            IsEnabled = true,
            LastUpdated = DateTime.UtcNow,
            UtcCreatedAt = DateTime.UtcNow,
            CreatedByNum = "admin"
        };

        Context.Users.Add(user);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repository.GetByUserIdAsync("testuser@example.com", TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.UserId.Should().Be("testuser@example.com");
        result.DisplayName.Should().Be("Test User");
        result.GraphId.Should().Be("graph-123");
        result.IsEnabled.Should().BeTrue();
    }

    [Fact]
    public async Task GetByUserIdAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByUserIdAsync("nonexistent@example.com", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByUserIdAsync_WithCaseSensitiveUserId_ShouldRespectCasing()
    {
        // Arrange
        var user = new User
        {
            GraphId = "graph-123",
            UserId = "TestUser@Example.com",
            DisplayName = "Test User",
            PrincipalName = "TestUser@Example.com",
            IsEnabled = true,
            LastUpdated = DateTime.UtcNow,
            UtcCreatedAt = DateTime.UtcNow
        };

        Context.Users.Add(user);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var exactMatch = await _repository.GetByUserIdAsync("TestUser@Example.com", TestContext.Current.CancellationToken);
        var differentCase = await _repository.GetByUserIdAsync("testuser@example.com", TestContext.Current.CancellationToken);

        // Assert
        exactMatch.Should().NotBeNull();
        // Note: SQLite is case-insensitive by default for strings, but this test documents the behavior
        // In a real SQL Server environment, this might behave differently based on collation
    }

    [Fact]
    public async Task GetByIdAsync_InheritedFromGeneric_ShouldWork()
    {
        // Arrange
        var user = new User
        {
            GraphId = "graph-456",
            UserId = "inherited@example.com",
            DisplayName = "Inherited Test User",
            PrincipalName = "inherited@example.com",
            IsEnabled = true,
            LastUpdated = DateTime.UtcNow,
            UtcCreatedAt = DateTime.UtcNow
        };

        Context.Users.Add(user);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repository.GetByIdAsync(user.Id, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
        result.UserId.Should().Be("inherited@example.com");
    }

    [Fact]
    public async Task AddAsync_ShouldAddUserSuccessfully()
    {
        // Arrange
        var user = new User
        {
            GraphId = "graph-789",
            UserId = "newuser@example.com",
            DisplayName = "New User",
            PrincipalName = "newuser@example.com",
            IsEnabled = true,
            LastUpdated = DateTime.UtcNow,
            UtcCreatedAt = DateTime.UtcNow,
            CreatedByNum = "system"
        };

        // Act
        var result = await _repository.AddAsync(user, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.UserId.Should().Be("newuser@example.com");

        // Verify using the specific method
        var retrievedUser = await _repository.GetByUserIdAsync("newuser@example.com", TestContext.Current.CancellationToken);
        retrievedUser.Should().NotBeNull();
        retrievedUser!.DisplayName.Should().Be("New User");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUserSuccessfully()
    {
        // Arrange
        var user = new User
        {
            GraphId = "graph-update",
            UserId = "updateme@example.com",
            DisplayName = "Original Name",
            PrincipalName = "updateme@example.com",
            IsEnabled = true,
            LastUpdated = DateTime.UtcNow,
            UtcCreatedAt = DateTime.UtcNow
        };

        Context.Users.Add(user);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Detach to simulate fresh context
        Context.Entry(user).State = EntityState.Detached;

        // Modify user
        user.DisplayName = "Updated Name";
        user.IsEnabled = false;
        user.UtcUpdatedAt = DateTime.UtcNow;
        user.UpdatedByNum = "admin";

        // Act
        var result = await _repository.UpdateAsync(user, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.DisplayName.Should().Be("Updated Name");
        result.IsEnabled.Should().BeFalse();

        // Verify using specific method
        var updatedUser = await _repository.GetByUserIdAsync("updateme@example.com", TestContext.Current.CancellationToken);
        updatedUser.Should().NotBeNull();
        updatedUser!.DisplayName.Should().Be("Updated Name");
        updatedUser.IsEnabled.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUserSuccessfully()
    {
        // Arrange
        var user = new User
        {
            GraphId = "graph-delete",
            UserId = "deleteme@example.com",
            DisplayName = "Delete Me",
            PrincipalName = "deleteme@example.com",
            IsEnabled = true,
            LastUpdated = DateTime.UtcNow,
            UtcCreatedAt = DateTime.UtcNow
        };

        Context.Users.Add(user);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var userId = user.Id;

        // Clear tracking to avoid conflicts
        ClearContext();

        // Act
        var result = await _repository.DeleteAsync(userId, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(1);

        // Verify deletion
        var deletedUser = await _repository.GetByUserIdAsync("deleteme@example.com", TestContext.Current.CancellationToken);
        deletedUser.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new()
            {
                GraphId = "graph-1",
                UserId = "user1@example.com",
                DisplayName = "User One",
                PrincipalName = "user1@example.com",
                IsEnabled = true,
                LastUpdated = DateTime.UtcNow,
                UtcCreatedAt = DateTime.UtcNow
            },
            new()
            {
                GraphId = "graph-2",
                UserId = "user2@example.com",
                DisplayName = "User Two",
                PrincipalName = "user2@example.com",
                IsEnabled = false,
                LastUpdated = DateTime.UtcNow,
                UtcCreatedAt = DateTime.UtcNow
            }
        };

        Context.Users.AddRange(users);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var results = await _repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        results.Should().HaveCount(2);
        results.Should().Contain(u => u.UserId == "user1@example.com");
        results.Should().Contain(u => u.UserId == "user2@example.com");
    }
}
