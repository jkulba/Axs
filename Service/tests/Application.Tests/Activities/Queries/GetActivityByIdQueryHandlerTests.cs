using Application.Activities.Queries;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;
using Moq;
using Xunit;

namespace Application.Tests.Activities.Queries;

public class GetActivityByIdQueryHandlerTests
{
    private readonly Mock<IActivityRepository> _mockActivityRepository;
    private readonly GetActivityByIdQueryHandler _handler;

    public GetActivityByIdQueryHandlerTests()
    {
        _mockActivityRepository = new Mock<IActivityRepository>();
        _handler = new GetActivityByIdQueryHandler(_mockActivityRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenActivityExists_ShouldReturnSuccessResult()
    {
        // Arrange
        var activityId = 1;
        var query = new GetActivityByIdQuery(activityId);
        var expectedActivity = new Activity
        {
            Id = activityId,
            ActivityCode = "ACT001",
            ActivityName = "Test Activity",
            Description = "Test Description",
            IsActive = true
        };

        _mockActivityRepository
            .Setup(x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedActivity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedActivity, result.Value);
        Assert.Equal(activityId, result.Value.Id);
        Assert.Equal("ACT001", result.Value.ActivityCode);
        Assert.Equal("Test Activity", result.Value.ActivityName);
        Assert.Equal("Test Description", result.Value.Description);
        Assert.True(result.Value.IsActive);

        _mockActivityRepository.Verify(
            x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenActivityDoesNotExist_ShouldReturnFailureResult()
    {
        // Arrange
        var activityId = 999;
        var query = new GetActivityByIdQuery(activityId);

        _mockActivityRepository
            .Setup(x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Activity?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ActivityErrors.ActivityByIdNotFound(activityId).Code, result.Error.Code);
        Assert.Equal(ActivityErrors.ActivityByIdNotFound(activityId).Description, result.Error.Description);

        _mockActivityRepository.Verify(
            x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidId_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var activityId = 42;
        var query = new GetActivityByIdQuery(activityId);
        var cancellationToken = new CancellationToken();
        var activity = new Activity { Id = activityId };

        _mockActivityRepository
            .Setup(x => x.GetByIdAsync(activityId, cancellationToken))
            .ReturnsAsync(activity);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _mockActivityRepository.Verify(
            x => x.GetByIdAsync(activityId, cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var activityId = 1;
        var query = new GetActivityByIdQuery(activityId);
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockActivityRepository
            .Setup(x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(query, CancellationToken.None));

        Assert.Equal(expectedException.Message, exception.Message);
        _mockActivityRepository.Verify(
            x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    public async Task Handle_WithVariousIds_ShouldPassCorrectIdToRepository(int activityId)
    {
        // Arrange
        var query = new GetActivityByIdQuery(activityId);

        _mockActivityRepository
            .Setup(x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Activity?)null);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockActivityRepository.Verify(
            x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenActivityFoundWithMinimalData_ShouldReturnCorrectResult()
    {
        // Arrange
        var activityId = 1;
        var query = new GetActivityByIdQuery(activityId);
        var expectedActivity = new Activity
        {
            Id = activityId,
            ActivityCode = "MIN",
            ActivityName = "Minimal Activity",
            Description = null, // Testing null description
            IsActive = false    // Testing inactive activity
        };

        _mockActivityRepository
            .Setup(x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedActivity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedActivity, result.Value);
        Assert.Equal(activityId, result.Value.Id);
        Assert.Equal("MIN", result.Value.ActivityCode);
        Assert.Equal("Minimal Activity", result.Value.ActivityName);
        Assert.Null(result.Value.Description);
        Assert.False(result.Value.IsActive);
    }

    [Fact]
    public async Task Handle_MultipleCallsWithSameId_ShouldCallRepositoryMultipleTimes()
    {
        // Arrange
        var activityId = 1;
        var query = new GetActivityByIdQuery(activityId);
        var activity = new Activity { Id = activityId };

        _mockActivityRepository
            .Setup(x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activity);

        // Act
        await _handler.Handle(query, CancellationToken.None);
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockActivityRepository.Verify(
            x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }
}
