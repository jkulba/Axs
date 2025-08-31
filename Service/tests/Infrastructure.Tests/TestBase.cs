using Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

public abstract class TestBase : IDisposable
{
    protected readonly AccessDbContext Context;
    private readonly SqliteConnection _connection;

    protected TestBase()
    {
        // Create an in-memory SQLite database
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AccessDbContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new AccessDbContext(options);

        // Ensure the database is created
        Context.Database.EnsureCreated();
    }

    protected void ClearContext()
    {
        Context.ChangeTracker.Clear();
    }

    public void Dispose()
    {
        Context?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}
