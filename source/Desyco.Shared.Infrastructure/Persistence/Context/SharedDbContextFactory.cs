using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Desyco.Shared.Infrastructure.Persistence.Context;

public class SharedDbContextFactory : IDesignTimeDbContextFactory<SharedDbContext>
{
    public SharedDbContext CreateDbContext(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../../Desyco.Dms.Web")) // Corrected path to startup project
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("Application"); // Use "Application" connection string

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'Application' not found in appsettings.json or is empty.");
        }

        var builder = new DbContextOptionsBuilder<SharedDbContext>();
        builder.UseSqlServer(connectionString,
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(SharedDbContext).Assembly.FullName);
            });

        return new SharedDbContext(builder.Options);
    }
}
