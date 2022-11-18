using DEVCoursesAPI.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DEVCoursesAPI.Tests;

public class TestCoursesDbContextFactory: IDbContextFactory<DEVCoursesContext>
{
    private readonly DbContextOptions<DEVCoursesContext> _options;

    public TestCoursesDbContextFactory(string databaseName = "InMemoryTest")
    {
        _options = new DbContextOptionsBuilder<DEVCoursesContext>()
            .UseInMemoryDatabase(databaseName, new InMemoryDatabaseRoot())
            .Options;
    }

    public DEVCoursesContext CreateDbContext()
    {
        return new DEVCoursesContext(_options);
    }
    
}