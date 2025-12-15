using Application.FunctionalTests.TestingInfrastructure;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace DartAppClean.Application.FunctionalTests.Repositories;
public abstract class RepositoryTestBase
{
    protected PostgreSqlContainer _pg;
    protected DbContextOptions<ApplicationDbContextTest> _options;

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        _pg = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        await _pg.StartAsync();

        _options = new DbContextOptionsBuilder<ApplicationDbContextTest>()
            .UseNpgsql(_pg.GetConnectionString())
            .EnableSensitiveDataLogging()
            .Options;

        using var ctx = new ApplicationDbContextTest(_options);
        await ctx.Database.EnsureCreatedAsync();
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        if (_pg != null)
            await _pg.DisposeAsync();
    }
    [SetUp]
    public async Task PerTestSetup()
    {
        using var ctx = new ApplicationDbContextTest(_options);

        await ctx.Database.ExecuteSqlRawAsync("""
        TRUNCATE TABLE "Teams", "Players", "Matches"
        RESTART IDENTITY CASCADE;
    """);
    }
}
