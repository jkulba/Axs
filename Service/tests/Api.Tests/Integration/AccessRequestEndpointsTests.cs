using Application.AccessRequests.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Api.Tests.Integration;

public class AccessRequestEndpointsTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AccessRequestEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public async ValueTask InitializeAsync()
    {
        await _factory.InitializeDatabaseAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _factory.CleanupDatabaseAsync();
    }

    [Fact]
    public async Task CreateAccessRequest_WithValidationErrors_ReturnsValidationProblemDetails()
    {
        // Arrange
        var command = new
        {
            requestCode = Guid.NewGuid(),
            userName = "", // Invalid - required field
            jobNumber = 0, // Invalid - must be positive
            cycleNumber = 1,
            activityCode = "", // Invalid - required field
            workstation = "", // Invalid - required field  
            applicationName = "TestApp",
            utcCreatedAt = DateTime.UtcNow
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/access-requests", command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Contains("validation", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("errors", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CreateAccessRequest_WithValidData_ReturnsCreated()
    {
        // Arrange
        var command = new
        {
            requestCode = Guid.NewGuid(),
            userName = "john.doe",
            jobNumber = 12345,
            cycleNumber = 1,
            activityCode = "TEST",
            workstation = "WS001",
            applicationName = "TestApp",
            utcCreatedAt = DateTime.UtcNow
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/access-requests", command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }
}
